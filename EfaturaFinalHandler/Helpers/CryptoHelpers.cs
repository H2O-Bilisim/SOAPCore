using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EfaturaFinalHandler.Helpers
{
    public class CryptoHelpers
    {
        MD5 md5;

        public CryptoHelpers()
        {
            md5 = new MD5CryptoServiceProvider();
        }
        public string GetMd5Hash(string Input, bool ToLower = false)
        {
            if (String.IsNullOrEmpty(Input))
                return string.Empty;

            byte[] InputByte = ObjectToByteArray(Input);
            string HashString = string.Empty;

            HashString = BitConverter.ToString(md5.ComputeHash(InputByte)).Replace("-", "");
            if (ToLower)
                return HashString.ToLower();

            return HashString;
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
}
