using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace EfaturaFinalHandler.Models 
{
    [DataContract]
    public class documentReturnType
    {
        [DataMember]
        public string msg { get; set; }
        [DataMember]
        public string hash { get; set; }
    }
}
