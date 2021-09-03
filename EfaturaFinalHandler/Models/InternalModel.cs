using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfaturaFinalHandler.Models
{
    public class InternalModel
    {
        public InternalModel()
        {
            doc_list = new List<Doclist>();
        }
        public List<Doclist> doc_list { get; set; }
    }
    public class Doclist
    {
        public string name { get; set; }
        public byte[] document { get; set; }
    }
}
