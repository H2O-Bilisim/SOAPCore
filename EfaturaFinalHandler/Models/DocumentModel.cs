using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace EfaturaFinalHandler.Models
{
    [DataContract]
    public class DocumentModel
    {
        [DataMember]
        public string fileName { get; set; }
        [DataMember]
        public byte[] binaryData { get; set; }
        [DataMember]
        public string hash { get; set; }
    }

    [DataContract]
    public class ResponseModel
    {
        [DataMember]
        public string msg { get; set; }
        [DataMember]
        public string hash { get; set; }
    }
}
