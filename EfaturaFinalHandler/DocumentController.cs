using EfaturaFinalHandler.Models;
using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Xml;

namespace EfaturaFinalHandler
{
    public class DocumentController
    {
        public int ValidateDocument(DocumentType document)
        {
            // Check all object property is null or empty
            if (IsNullOrEmpty(document.fileName) || IsNullOrEmpty(document.hash) || IsNullOrEmpty(document.binaryData))
            {
                return 2000;
            }

            // Assign objects property to proper variable
            string fileName = document.fileName;
            string hash = document.hash;
            byte[] binaryData = FromBase64String(document.binaryData);
            
            // Call and use memory stream to store binary data on memory
            using (MemoryStream memStream = new MemoryStream(binaryData))
            {
                // Call MD5 library from System.Security.Cryptography
                using (var md5 = MD5.Create())
                {
                    // Use MD5 library to convert byte array to MD5 string
                    var binaryDataHash =  md5.ComputeHash(memStream).ToString();
                    
                    // Check the binaryData is matched with hash
                    if(binaryDataHash != hash)
                    {
                        return 2000;
                    }
                }
                // Try to read and convert binary data to ZipArchive
                try
                {
                    using (ZipArchive archive = new ZipArchive(memStream, ZipArchiveMode.Read, false))
                    {
                        // Check the archive name is matched with file name
                        string archiveName = archive.Entries[0].FullName.Split('.')[0];
                        if (archiveName != filename)
                        {
                            return 2004;
                        }

                        // Endpoint to check if the envelope is exists
                        var h = new H2oServiceRequester();
                        var login = h.Login();
                        var ReturnOfService = h.CheckIncomingEnvelope(filename);
                        if (ReturnOfService.envelope_id != filename)
                        {
                            return 2001;
                        }

                        foreach(var item in archive.Entries)
                        {
                            string archiveContent = string.Empty;
                            var zstream = new StreamReader(item.Open(), Encoding.UTF8);
                            string ztempstr = zstream.ReadToEnd();

                            // Read archive item as xml document
                            XmlDocument xml = new XmlDocument();
                            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
                            
                            // Get UUID from xml document
                            string xPathString = "//cbc:UUID";
                            XmlNode uuid = xml.DocumentElement.SelectSingleNode(xPathString, nsmgr);

                            // Check UUID if its valid
                            Guid parsedGuid = new Guid();
                            Guid.TryParse(stringGuid, out parsedGuid);
                            if (uuid != parsedGuid)
                            {
                                return 2006;
                            }

                            archiveContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(ztempstr));
                        }
                    }
                }
                catch (System.Exception)
                {
                    return 2004;
                }
            }

            var ReturnOfService = h.SaveIncomingFile(document);
            if( ReturnOfService.GetType()  != typeof(string))
            {
                return 1;
            }
            else
            {
                return 2003;
            }

            return 0;
        }
    }
}