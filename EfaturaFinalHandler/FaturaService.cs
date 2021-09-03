using EfaturaFinalHandler.Models;
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
using System.Threading.Tasks;

namespace EfaturaFinalHandler
{
    public class FaturaService : IFaturaService
    {
        public documentReturnType sendDocument(documentType document)
        {
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
                        return response;
                    }

                   
                    using (var archive = new ZipArchive(stream))
                    {
                        foreach(var item in archive.Entries)
                        {
                            var ModelData = new Doclist();
                            var zipfile = archive.GetEntry(item.Name);
                            using (var zstream = item.Open())
                            {
                                using (var reader = new StreamReader(zstream))
                                {
                                    using (var memstream = new MemoryStream())
                                    {
                                        var buffer = new byte[512];
                                        var bytesRead = default(int);
                                        while ((bytesRead = reader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                                            memstream.Write(buffer, 0, bytesRead);
                                       ModelData.document = memstream.ToArray();
                                    }
                                }
                            }
                            ModelData.name = item.FullName;
                            ProcessModel.doc_list.Add(ModelData);
                        }


                    }
                }
            }

            var ReturnOfService = h.SaveIncomingFile(ProcessModel);
           
            return response;
            //return string.Join(string.Empty, msg.Reverse());

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
                byte[] result = md5.ComputeHash(objectAsBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    sb.Append(result[i].ToString("X2"));
                }

                // And return it
                return sb.ToString();
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
