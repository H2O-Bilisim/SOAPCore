using EfaturaFinalHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace EfaturaFinalHandler
{
    public class FaturaService: IFaturaService
    {
        public ResponseModel sendDocument(DocumentModel document)
        {
            var response = new ResponseModel { hash = "TESTHASH", msg = "testMSG" };
            return response;
            //return string.Join(string.Empty, msg.Reverse());
        }
       
    }

   
    [ServiceContract]
    public interface IFaturaService
    {
        [OperationContract]
        ResponseModel sendDocument(DocumentModel document);

    }
    
}
