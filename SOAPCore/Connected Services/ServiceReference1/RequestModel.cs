using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoapCore.Connected_Services.ServiceReference1
{
    public class RequestModel
    {
        public string fileName { get; set; }
        public byte[] binaryData { get; set; }
        public string hash { get; set; }
    }
}
