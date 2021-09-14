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
using System.Text.Json;

namespace EfaturaFinalHandler
{
    public class FaturaService : IFaturaService
    {
        private ThreadLocal<string> _paramValue = new ThreadLocal<string>() { Value = string.Empty };

        public Object sendDocument(documentType document)
        {
            try
            {
                LogWriter log = new LogWriter();
                log.Requestci(document);

                DocumentController documentController = new DocumentController();
                int validCode = documentController.ValidateDocument(document);

                documentReturn documentResponse = new documentReturn();
                EFaturaFault faultResponse = new EFaturaFault();

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
            catch (FaultException ex)
            {
                var fault = ex.CreateMessageFault();
                var FaultModel = new documentTypeFault();
                FaultModel.faultCode = int.Parse(fault.Code.ToString());
                FaultModel.faultMsg = fault.Reason.ToString();
                return fault;
            }
            
        }
        public Object getApplicationResponse(getAppRespRequestType instanceIdentifier)
        {
            try
            {
                var h = new H2oServiceRequester();
                LogWriter log = new LogWriter();
                log.Requestci(instanceIdentifier);

                getAppRespResponse appResponse = new getAppRespResponse();
                EFaturaFault faultResponse = new EFaturaFault();

                var ReturnOfService = h.CheckIncomingEnvelope(instanceIdentifier);
                getAppRespResponseType appRespResponse = JsonSerializer.Deserialize(ReturnOfService);
                if (appRespResponse.applicationResponse == "ZARF ID BULUNAMADI")
                {
                    //log.Responscu(faultResponse.getResponse(2004));
                    return faultResponse.getResponse(2004);
                }
                //log.Responscu(appResponse.getResponse(appRespResponse));
                return appResponse.getResponse(appRespResponse.applicationResponse);
            }
            catch(FaultException ex)
            {
                var fault = ex.CreateMessageFault();
                var FaultModel = new getAppRespRequestTypeFault();
                FaultModel.faultCode = int.Parse(fault.Code.ToString());
                FaultModel.faultMsg = fault.Reason.ToString();
                return fault;
            }
            
        }
    }

    [ServiceContract]
    public interface IFaturaService
    {
        [OperationContract]
        [FaultContract(typeof(documentTypeFault))]
        Object getApplicationResponse(getAppRespRequestType instanceIdentifier);

        [OperationContract]
        [FaultContract(typeof(getAppRespRequestTypeFault), Action = "sendDocumentFault", Name = "sendDocumentFault", Namespace = "sendDocument") ]
        Object sendDocument(documentType document);
    }

    
}
