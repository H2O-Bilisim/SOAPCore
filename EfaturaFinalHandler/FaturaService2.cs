﻿using EfaturaFinalHandler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EfaturaFinalHandler
{
    public class FaturaService : IFaturaService
    {
        private ThreadLocal<string> _paramValue = new ThreadLocal<string>() { Value = string.Empty };
        public documentReturnType sendDocument(documentType document)
        {
            LogWriter log = new LogWriter();
            log.Requestci(document);
            var h = new H2oServiceRequester();
            var login = h.Login();
            var response = new documentReturnType();
            var ProcessModel = new InternalModel();
            ProcessModel.doc_list = new List<Doclist>();
            using (var md5 = MD5.Create())
            {
                using (MemoryStream stream = new MemoryStream(document.binaryData))
                {
                    var md5ComputeHash =  GenerateKey(document.binaryData);
                    
                    if(md5ComputeHash != document.hash)
                    {
                        response.hash = md5ComputeHash;
                        response.msg = "Incoming File Hash not MD5 Format or not Correct";
                        log.Responscu(response);
                        return response;
                    }

                   
                    using (var archive = new ZipArchive(stream))
                    {
                        foreach(var item in archive.Entries)
                        {
                            var ModelData = new Doclist();
                            string zipfileContent = string.Empty;
                            var zstream = new StreamReader(item.Open(), Encoding.UTF8);
                            string ztempstr = zstream.ReadToEnd();
                            zipfileContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(ztempstr));
                            //using (var zstream = item.Open())
                            //{
                            //    using (var reader = new StreamReader(zstream))
                            //    {
                            //        using (var memstream = new MemoryStream())
                            //        {
                            //            var buffer = new byte[512];
                            //            var bytesRead = default(int);
                            //            while ((bytesRead = reader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                            //                memstream.Write(buffer, 0, bytesRead);
                            //           ModelData.document = memstream.ToArray();
                            //        }
                            //    }
                            //}
                            ModelData.name = item.FullName;
                            ModelData.document = zipfileContent;
                            ProcessModel.doc_list.Add(ModelData);
                        }


                    }
                }
            }


            var ReturnOfService = h.SaveIncomingFile(ProcessModel);
            if( ReturnOfService.GetType()  != typeof(string))
            {
                response.msg = "ZARF KUYRUGA EKLENDI";
                response.hash = "";
            }
            else
            {
                response.msg = "SISTEM HATASI";
                response.hash = "";
            }
            log.Responscu(response);
            return response;
            //return string.Join(string.Empty, msg.Reverse());
            /*
             * 
             * Doc_list:
             * filename: "name.xml"
             * file: "base64Encodedfilecontent"
             */

        }

        public static String GenerateKey(Object sourceObject)
        {
            String hashString;

            if (sourceObject == null)
            {
                throw new ArgumentNullException("Null as parameter is not allowed");
            }
            else
            {
                try
                {
                    hashString = ComputeHash(ObjectToByteArray(sourceObject));
                    return hashString;
                }
                catch (AmbiguousMatchException ame)
                {
                    throw new ApplicationException("Could not definitely decide if object is serializable.Message:" + ame.Message);
                }
            }
        }

        private static string ComputeHash(byte[] objectAsBytes)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            try
            {
                var Hash = BitConverter.ToString(md5.ComputeHash(objectAsBytes)).Replace("-", "").ToLowerInvariant();

                
                return Hash;
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("Hash has not been generated.");
                return null;
            }
        }

        private static readonly Object locker = new Object();

        private static byte[] ObjectToByteArray(Object objectToSerialize)
        {
            MemoryStream fs = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                //Here's the core functionality! One Line!
                //To be thread-safe we lock the object
                lock (locker)
                {
                    formatter.Serialize(fs, objectToSerialize);
                }
                return fs.ToArray();
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Error occurred during serialization. Message: " +
                se.Message);
                return null;
            }
            finally
            {
                fs.Close();
            }
        }
    }


        [ServiceContract]
    public interface IFaturaService
    {
        [OperationContract]
        documentReturnType sendDocument(documentType document);

    }
    
}
