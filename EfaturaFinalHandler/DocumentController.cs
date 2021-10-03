using EfaturaFinalHandler.Helpers;
using EfaturaFinalHandler.Models;
using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace EfaturaFinalHandler
{
    public class DocumentController
    {
        private CryptoHelpers _ch;
        public DocumentController()
        {
            _ch = new CryptoHelpers();
        }
        public void ValidateDocument(documentRequest document)
        {
            EFaturaFaultType fault = new EFaturaFaultType();
            
            // Check all object property is null or empty
            if (string.IsNullOrEmpty(document.fileName) || string.IsNullOrEmpty(document.hash) || string.IsNullOrEmpty(Convert.ToBase64String(document.binaryData)))
            {
                fault.throwResponse("2000");
            }

            // Assign objects property to proper variable
            string fileName = document.fileName.Replace(".zip","");
            string hash = document.hash;
            byte[] binaryData = document.binaryData;

            using (MD5 md5 =  MD5.Create())
            {
                string binaryHash = BitConverter.ToString(md5.ComputeHash(binaryData)).Replace("-", "");
                
                if (hash != binaryHash)
                {
                    fault.throwResponse("2000");
                }
            }

            // Endpoint to check if the envelope is exists
            var h = new H2oServiceRequester();
            var login = h.Login();

            var appRespResponse = new getAppRespResponseType();
            var response = appRespResponse.getResponse(fileName);
            if (response.applicationResponse !=  "ZARF ID BULUNAMADI")
            {
                fault.throwResponse("2001");
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
                                fault.throwResponse("2000");
                            }
                            string archiveContent = string.Empty;
                            var zstream = new StreamReader(item.Open(), Encoding.UTF8);
                            string ztempstr = zstream.ReadToEnd();

                            // Read archive item as xml document
                            var xml = new XmlDocument();
                            xml.LoadXml(ztempstr);
                            var nsmgr = new XmlNamespaceManager(xml.NameTable);
                            nsmgr.AddNamespace("sh", "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader");
                            nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

                            XmlNode instanceIdentifier;
                            XmlElement root = xml.DocumentElement;
                            instanceIdentifier = root.SelectSingleNode("//sh:InstanceIdentifier", nsmgr);
                            var instanceIdentifierText = instanceIdentifier.InnerText;
                            if (instanceIdentifierText != fileName)
                            {
                                fault.throwResponse("2004");
                            }

                            // Get UUID from xml document
                            XmlNode uuid = root.SelectSingleNode("//cbc:UUID", nsmgr);

                            // Check UUID if its valid
                            Guid parsedGuid = new Guid();
                            var parseResult = Guid.TryParse(uuid.InnerText, out parsedGuid);
                            if (!parseResult)
                            {
                                fault.throwResponse("2006");
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
                    fault.throwResponse("2006");
                }
            }

            var send_file_response = h.SaveIncomingFile(ProcessModel);
            if(send_file_response.status  != "ZARF KUYRUGA EKLENDI")
            {
                fault.throwResponse("2005");
            }
        }
    }
}