using EfaturaFinalHandler.Helpers;
using EfaturaFinalHandler.Models;
using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Text.Json;
using System.ServiceModel;

namespace EfaturaFinalHandler
{
    public class DocumentController
    {
        private CryptoHelpers _ch;
        public DocumentController()
        {
            _ch = new CryptoHelpers();
        }
        public int ValidateDocument(documentType document)
        {
            // Check all object property is null or empty
            if (string.IsNullOrEmpty(document.fileName) || string.IsNullOrEmpty(document.hash) || string.IsNullOrEmpty(Convert.ToBase64String(document.binaryData)))
            {
                throw new FaultException("2000");
            }

            // Assign objects property to proper variable
            string fileName = document.fileName;
            string hash = document.hash;
            byte[] binaryData = document.binaryData;

            if(hash != _ch.GetMd5Hash(Convert.ToBase64String(document.binaryData)))
            {
                throw new FaultException("2000");
            }

            // Endpoint to check if the envelope is exists
            var h = new H2oServiceRequester();
            var login = h.Login();

            var requestObj = new getAppRespRequestType();
            requestObj.instanceIdentifier = document.fileName;

            var ReturnOfService = h.CheckIncomingEnvelope(requestObj);
            getAppRespResponseType appRespResponse = JsonSerializer.Deserialize(ReturnOfService);
            if(appRespResponse.applicationResponse !=  "ZARF ID BULUNAMADI")
            {
                throw new FaultException("2004");
            }
            var ProcessModel = new InternalModel();
            // Call and use memory stream to store binary data on memory
            using (MemoryStream memStream = new MemoryStream(binaryData))
            {
                try
                {
                    using (ZipArchive archive = new ZipArchive(memStream, ZipArchiveMode.Read, false))
                    {
                        foreach(var item in archive.Entries)
                        {
                            if(item.FullName.Split('.')[0] != fileName)
                            {
                                throw new FaultException("2004");
                            }
                            string archiveContent = string.Empty;
                            var zstream = new StreamReader(item.Open(), Encoding.UTF8);
                            string ztempstr = zstream.ReadToEnd();

                            // Read archive item as xml document
                            XmlDocument xml = new XmlDocument();
                            xml.Load(zstream);
                            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
                            
                            string xPathString = "//sh:InstanceIdentifier";
                            XmlNode instanceIdentifier = xml.DocumentElement.SelectSingleNode(xPathString, nsmgr);
                            if(instanceIdentifier.InnerText != fileName)
                            {
                                throw new FaultException("2004");
                            }

                            // Get UUID from xml document
                            xPathString = "//cbc:UUID";
                            XmlNode uuid = xml.DocumentElement.SelectSingleNode(xPathString, nsmgr);

                            // Check UUID if its valid
                            Guid parsedGuid = new Guid();
                            
                            if (!Guid.TryParse(uuid.InnerText, out parsedGuid))
                            {
                                throw new FaultException("2006");
                            }

                            archiveContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(ztempstr));
                            
                            var ModelData = new Doclist();
                            ModelData.name = item.FullName;
                            ModelData.document = archiveContent;
                            
                            ProcessModel.doc_list.Add(ModelData);
                        }
                    }
                }
                catch (Exception)
                {
                    throw new FaultException("2004");
                }
            }

            var ServiceReturn = h.SaveIncomingFile(ProcessModel);
            if(ServiceReturn.GetType()  != typeof(string))
            {
                return 1;
            }
            else
            {
                throw new FaultException("2005");
            }

        }
    }
}