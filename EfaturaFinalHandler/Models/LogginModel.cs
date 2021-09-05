using System;
namespace EfaturaFinalHandler.Models
{
    public class LogginModel
    {

        public DateTime LogDate { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public string Host { get; set; }
        public string ClientIp { get; set; }
    }
}
