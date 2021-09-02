using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoapCore.Connected_Services.ServiceReference1;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace SoapCore.Connected_Services.ServiceReference1
{
    public class RestConnector
    {
        private string ConvertToJSON(RequestModel Model)
        {
            string JsonData = JsonConvert.SerializeObject(Model);

            return JsonData;
        }
        // async could not be found?
        public async Task<async> SendRequestAsync (RequestModel Request)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("Base Address/URL Address"); // Add Istanbul backend URL

                // serialize your json using newtonsoft json serializer then add it to the StringContent
                string RequestData = ConvertToJSON(Request);
                var content = new StringContent(RequestData, Encoding.UTF8, "application/json");

                // method address would be like api/callUber:SomePort for example
                var result = await client.PostAsync("Method Address", content); // Make a new endpoint for new required operations on devportal
                string resultContent = await result.Content.ReadAsStringAsync();
                
                return resultContent; // return what?
            }
        }

    }
}
