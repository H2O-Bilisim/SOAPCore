using System;
using System.IO;
using System.Threading.Tasks;
using EfaturaFinalHandler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EfaturaFinalHandler
{
    public sealed class RequestHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public RequestHandlerMiddleware(ILogger<RequestHandlerMiddleware> logger, RequestDelegate next)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string Path = @"./ServiceLogs.txt";
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            if (File.Exists(Path))
            {
                var Model = new LogginModel
                {
                    LogDate = DateTime.Now,
                    Header = JsonConvert.SerializeObject(context.Request.Headers, Formatting.Indented),
                    Body = body.ToString(),
                    Host = context.Request.Host.Host,
                    ClientIp = context.Connection.RemoteIpAddress.ToString()
                };

                var WriteData = JsonConvert.SerializeObject(Model);
                using (StreamWriter sw = File.AppendText(Path))
                {
                    sw.WriteLine(WriteData);
                }

            }
           
            await next(context);
        }

    }
}
