using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace EfaturaFinalHandler.Models
{
    [DataContract]
    public class getAppRespResponse
    {
        [DataMember]
        public string applicationResponse { get; set; }
    }
    
    [DataContract]
    public class documentResponse
    {
        [DataMember]
        public string msg { get; set; }
        [DataMember]
        public string hash { get; set; }
    }

    [DataContract]
    public class EFaturaFault
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public string msg { get; set; }
        public Dictionary<FaultCode, FaultReason> faultTypes;
    }
}
