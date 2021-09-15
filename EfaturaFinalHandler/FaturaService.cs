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
using System.Net.Security;

namespace EfaturaFinalHandler
{
    public class FaturaService : IFaturaService
    {
        private ThreadLocal<string> _paramValue = new ThreadLocal<string>() { Value = string.Empty };

        public documentReturnType sendDocument(documentType document)
        {
            try
            {
                LogWriter log = new LogWriter();
                log.Requestci(document);

                DocumentController documentController = new DocumentController();
                int validCode = documentController.ValidateDocument(document);

                documentReturn documentResponse = new documentReturn();
                EFaturaFault faultResponse = new EFaturaFault();

                switch (validCode)
                {
                    case 0:
                        return documentResponse.getResponse(validCode);
                    case 1:
                        return documentResponse.getResponse(validCode);
                    case 2000:
                        throw faultResponse.getResponse(validCode);
                    case 2001:
                        throw faultResponse.getResponse(validCode);
                    case 2003:
                        throw faultResponse.getResponse(validCode);
                    case 2004:
                        throw faultResponse.getResponse(validCode);
                    case 2006:
                        throw faultResponse.getResponse(validCode);
                    default:
                        throw faultResponse.getResponse();
                }
                log.Responscu(response);
            }
            catch (FaultException)
            {
                throw faultResponse.getResponse(2005);
                // var fault = ex.CreateMessageFault();
                // var FaultModel = new documentTypeFault();
                // FaultModel.faultCode = int.Parse(fault.Code.ToString());
                // FaultModel.faultMsg = fault.Reason.ToString();
                // return fault;
            }
            
        }
        public getAppRespResponseType getApplicationResponse(getAppRespRequestType instanceIdentifier)
        {
            FaultException exception = new FaultException();
            var fault = exception.CreateMessageFault();
            
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
                    throw faultResponse.getResponse(2004);
                }
                return appResponse.getResponse(appRespResponse.applicationResponse);
            }
            catch(FaultException ex)
            {
                throw faultResponse.getResponse(2005);
                // var fault = ex.CreateMessageFault();
                // var FaultModel = new getAppRespRequestTypeFault();
                // FaultModel.faultCode = int.Parse(fault.Code.ToString());
                // FaultModel.faultMsg = fault.Reason.ToString();
                // return fault;
            }
            
        }
    }

    [ServiceContract]
    public interface IFaturaService
    {
        [OperationContract]
        [FaultContract(typeof(EFaturaFaultType), Action = "http://tempuri.org/IFaturaService/getApplicationResponse", Name = "EFaturaFaultType")]
        getAppRespResponseType getApplicationResponse(getAppRespRequestType instanceIdentifier);

        [OperationContract]
        [FaultContract(typeof(EFaturaFaultType), Action = "http://tempuri.org/IFaturaService/sendDocument",Name ="EFaturaFaultType")]
        documentReturnType sendDocument(documentType document);
    }

    
}
