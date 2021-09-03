using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoapCore.Connected_Services.ServiceReference1;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Net;
using System.IO;

namespace SoapCore.Connected_Services.ServiceReference1
{
    public class RestConnector
    {
        public static string BaseUrl = "";
        public static string TokenUrl = "api/token/";
        public static string EndpointUrl = "api/save-incoming/";

        private static Dictionary<string,string>;

        private string ConvertToJSON(RequestModel Model)
        {
            string JsonData = JsonConvert.SerializeObject(Model);

            return JsonData;
        }

        private string ConvertToJSON(AuthData Model)
        {
            string JsonData = JsonConvert.SerializeObject(Model);

            return JsonData;
        }

        private Dictionary<string,string> FetchTokenPair()
        {
            AuthData AuthModel = new AuthData { username = "huseroz", password = "11221122" };
            string AuthJson = ConvertToJSON(AuthModel);

            WebRequest request = WebRequest.Create(BaseUrl + EndpointUrl);
            byte[] RequestBytes = Encoding.UTF8.GetBytes(RequestString);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = RequestBytes.Length;

            using var reqStream = request.GetRequestStream();
            reqStream.Write(RequestBytes, 0, RequestBytes.Length);

            using var response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            using var responseStream = response.GetResponseStream();

            using var reader = new StreamReader(responseStream);
            string data = reader.ReadToEnd();

            return data;
        }

        public string SendRequest(RequestModel Model)
        {
            string RequestString = ConvertToJSON(Model);

            WebRequest request = WebRequest.Create(BaseUrl + EndpointUrl);
            byte[] RequestBytes = Encoding.UTF8.GetBytes(RequestString);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = RequestBytes.Length;

            using var reqStream = request.GetRequestStream();
            reqStream.Write(RequestBytes, 0, RequestBytes.Length);

            using var response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            
            using var responseStream = response.GetResponseStream();

            using var reader = new StreamReader(responseStream);
            string data = reader.ReadToEnd();

            return data;
        }

    }

    public class AuthData
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
