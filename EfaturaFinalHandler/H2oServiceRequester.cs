using EfaturaFinalHandler.Models;
using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.IO;
using System.Net;

namespace EfaturaFinalHandler
{
    public class H2oServiceRequester
    {
        public  string Token = null;
        public string ApiHost = "https://portalapi.devdonusum.com/api/";
        public  string GetToken = "token/";
        public  string SaveIncoming = "save-incoming/";
        private string ApiUser = "huseroz";
        private string ApiPass = "11221122";
        public H2oServiceRequester()
        {
            //var configuration = new IConfiguration();
            /*
             * "ApiRoot": "https://portalapi.devdonusum.com/api/",
                "AuthEndPoint": "token",
                "SaveIncoming": "save-incoming",
                "ApiUser": "huseroz",
                "ApiPass": "11221122"
            */
            //ApiHost = System.Configuration.ConfigurationManager.AppSettings["ApiRoot"];
            //GetToken = System.Configuration.ConfigurationManager.AppSettings["AuthEndPoint"];
            //SaveIncoming = System.Configuration.ConfigurationManager.AppSettings["SaveIncoming"];
            //ApiUser = System.Configuration.ConfigurationManager.AppSettings["ApiUser"];
            //ApiPass = System.Configuration.ConfigurationManager.AppSettings["ApiPass"];
        }


        public  string Login()
        {
            if (String.IsNullOrEmpty(Token))
            {
                ServiceLogin();
                if (String.IsNullOrEmpty(Token))
                {
                    return null;
                }
                else
                {
                    return Token;
                }
            }
            else
            {
                return Token;
            }
        }
        
        public dynamic SaveIncomingFile(InternalModel model)
        {
            var Req = JsonConvert.SerializeObject(model);
            var login = Login();
            return SunucuSorgu(req: Req, Ek: SaveIncoming, Key: 1);
        }
        public dynamic CheckIncomingEnvelope(getAppRespRequest request)
        {
            var Req = JsonConvert.SerializeObject(request);
            var login = Login();
            return SunucuSorgu(req: Req, Ek: "check-incoming-envelope/", Key: 1);
        }
        public  void ServiceLogin()
        {
            if (String.IsNullOrEmpty(Token))
            {
                dynamic request = new ExpandoObject();
                request.username = ApiUser;
                request.password = ApiPass;




                dynamic resp = SunucuSorgu(request,GetToken);
                if (resp == null)
                {
                    Console.WriteLine("Login Servislerine Bağlanılamadı");
                    Console.Read();
                    Environment.Exit(1);
                }
                else
                {
                    Token = resp.access;
                }
            }
        }
        
        public  dynamic SunucuSorgu(dynamic req, string Ek =null, int Key = 0)
        {
            string jsonWS = "";
            
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(ApiHost + Ek);
                httpWebRequest.ContentType = "application/json";
                if (!String.IsNullOrEmpty(Token))
                {
                    httpWebRequest.Headers.Add("Authorization", "Bearer " + Token);
                }
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = 300000;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    if (Key == 0)
                    {
                        jsonWS = JsonConvert.SerializeObject(req);
                        
                    }
                    else
                    {
                        jsonWS = req;
                        
                    }

                    streamWriter.Write(jsonWS);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    dynamic ret = JsonConvert.DeserializeObject<ExpandoObject>(result);
                    return ret;
                }
            }
            catch (Exception e)
            {
                return "sendMessageToServer --- sunucu: " + ApiHost + Ek + " --- json: " + jsonWS + e.ToString();

            }
        }
    }
}
