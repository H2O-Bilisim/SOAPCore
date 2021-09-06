using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using EfaturaFinalHandler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EfaturaFinalHandler
{
    public  class RequestHandlerMiddleware
    {


        private readonly RequestDelegate next;
        //    private readonly ILogger logger;

        public RequestHandlerMiddleware(ILogger<RequestHandlerMiddleware> logger, RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            var Body = context.Request.Body;
            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            try
            {
                await next(context);
            }
            finally
            {
                
                var TempBody = Body;
                
                await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                var bodyAsText = Encoding.UTF8.GetString(buffer);
                var Header = JsonConvert.SerializeObject(context.Request.Headers, Formatting.Indented);
                var Host = context.Request.Host.Host;
                var Client = context.Connection.RemoteIpAddress.ToString();
                var BodyString = await new StreamReader(Body).ReadToEndAsync();
                string Path = @"ServiceLogs.txt";
            }
            
        }
        public async Task WriteLog(HttpContext Data, Stream bdy)
        {
            var BodyString = await new StreamReader(Data.Request.Body).ReadToEndAsync();
           

            var body = await new StreamReader(bdy).ReadToEndAsync();
            var Header = JsonConvert.SerializeObject(Data.Request.Headers, Formatting.Indented);
            var Host = Data.Request.Host.Host;
            var Client = Data.Connection.RemoteIpAddress.ToString();
            string Path = @"ServiceLogs.txt";
            if (File.Exists(Path))
            {
                var Model = new LogginModel
                {
                    LogDate = DateTime.Now,
                    Header = Header,
                    Body = BodyString,
                    Host = Host,
                    ClientIp = Client,
                };

                var WriteData = JsonConvert.SerializeObject(Model);
                using (StreamWriter sw = File.AppendText(Path))
                {
                    sw.WriteLine(WriteData);
                }

            }

        }
        //    public void WriteLog()
        //    {
        //        string Path = @"./ServiceLogs.txt";

        //        if (File.Exists(Path))
        //        {
        //            var Model = new LogginModel
        //            {
        //                LogDate = DateTime.Now,
        //                Header = "",
        //                Body = "",
        //                Host = "",
        //                ClientIp = "",
        //            };

        //            var WriteData = JsonConvert.SerializeObject(Model);
        //            using (StreamWriter sw = File.AppendText(Path))
        //            {
        //                sw.WriteLine(WriteData);
        //            }

        //        }
        //    }
        //}
    }
}