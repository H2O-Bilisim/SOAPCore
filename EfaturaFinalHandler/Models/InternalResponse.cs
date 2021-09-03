using System;
using System.Collections.Generic;

namespace EfaturaFinalHandler.Models
{
    
    public class InternalResponse
    {
        public InternalResponse()
        {
            result = new Dictionary<string, uuidDetails>();
        }
        public Dictionary<string, uuidDetails> result { get; set; }
    }
    public class uuidDetails
    {

        public string vault_doc { get; set; }
        public string msg { get; set; }
        public string hash { get; set; }
        public string vault_doc_uuid { get; set; }
        
    }
}
