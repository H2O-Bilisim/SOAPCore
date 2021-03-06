using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace EfaturaFinalHandler.Models
{
    [DataContract]
    public class documentRequest
    {
        [DataMember]
        public string fileName { get; set; }

        [DataMember] 
        public byte[] binaryData { get; set; }

        [DataMember]
        public string hash { get; set; }
    }

    [DataContract]
    public class getAppRespRequest
    {
        [DataMember]
        public string instanceIdentifier { get; set; }
    }
}
