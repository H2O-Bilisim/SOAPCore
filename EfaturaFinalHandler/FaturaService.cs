using EfaturaFinalHandler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EfaturaFinalHandler
{
    public class FaturaService : IFaturaService
    {
        private ThreadLocal<string> _paramValue = new ThreadLocal<string>() { Value = string.Empty };
        public Object sendDocument(DocumentType document)
        {
            LogWriter log = new LogWriter();
            //log.Requestci(document);

            var h = new H2oServiceRequester();
            var login = h.Login();

            DocumentResponse documentResponse = new DocumentResponse();
            FaultResponse faultResponse = new FaultResponse();

            DocumentController documentController = new DocumentController();
            int validCode = documentController.ValidateDocument(document);

            dynamic response;
            switch (validCode)
            {
                case 0:
                    response = documentResponse.getResponse(validCode);
                    break;
                case 1:
                    response = documentResponse.getResponse(validCode);
                    break;
                case 2000:
                    response = faultResponse.getResponse(validCode);
                    break;
                case 2001:
                    response = faultResponse.getResponse(validCode);
                    break;
                case 2003:
                    response = faultResponse.getResponse(validCode);
                    break;
                case 2004:
                    response = faultResponse.getResponse(validCode);
                    break;
                case 2006:
                    response = faultResponse.getResponse(validCode);
                    break;
                default:
                    response = faultResponse.getResponse();
                    break;
            }
            log.Responscu(response);
            return response;
        }
        public Object sendDocument(AppRespRequestType document)
        {
            var response = new AppRespResponse();
            return response;
        }
    }

    [ServiceContract]
    public interface IFaturaService
    {
        [OperationContract]
        Object sendDocument(DocumentType document);
        Object sendDocument(AppRespRequestType document);
    }
}
