using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace EfaturaFinalHandler
{
    public class FaturaService
    {
        public string Ping(string msg)
        {
            return string.Join(string.Empty, msg.Reverse());
        }
        public void Pong(double s)
        {
            
        }
    }

   
    [ServiceContract]
    public interface IFaturaService
    {
        [OperationContract]
        string Ping(string msg);

        [OperationContract]
        void Pong(string s);
    }
    
}
