using System;
using System.IO;
using EfaturaFinalHandler.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EfaturaFinalHandler
{
    public class LogWriter
    {
        public LogWriter()
        {
        }
        public void Logcu(string body, HttpContext context, string client)
        {
            // X-Real-IP
            var RealIp = context.Request.Headers["X-Real-IP"];
            
            var model = new LogginModel
            {
                LogDate = DateTime.Now,
                Body = body,
                ClientIp = RealIp,
                Header = JsonConvert.SerializeObject(context.Request.Headers, Formatting.Indented),

            };
            string Path = @"ServiceLogs.txt";
            if (File.Exists(Path))
            {
               

                var WriteData = JsonConvert.SerializeObject(model);
                using (StreamWriter sw = File.AppendText(Path))
                {
                    sw.WriteLine(WriteData);
                }

            }
        }
        public void Requestci(documentType model)
        {
            string Path = @"RequestLog.txt";
            if (File.Exists(Path))
            {


                var WriteData = JsonConvert.SerializeObject(model);
                using (StreamWriter sw = File.AppendText(Path))
                {
                    sw.WriteLine(WriteData);
                }

            }
        }

        public void Responscu(documentReturnType model)
        {
            string Path = @"ResponseLog.txt";
            if (File.Exists(Path))
            {


                var WriteData = JsonConvert.SerializeObject(model);
                using (StreamWriter sw = File.AppendText(Path))
                {
                    sw.WriteLine(WriteData);
                }

            }
        }
    }
}
