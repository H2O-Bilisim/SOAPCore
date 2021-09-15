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

                switch (validCode)
                {
                    case 0:
                        return documentResponse.getResponse(validCode);
                    case 1:
                        return documentResponse.getResponse(validCode);
                    default:
                        throw new FaultException("2005");
                }
            }
            catch (FaultException faultCode)
            {
                EFaturaFault faultResponse = new EFaturaFault();
                var fault = faultResponse.getResponse(Convert.ToInt32(faultCode)); 
                throw new FaultException<EFaturaFaultType>(fault);
            }
            
        }
        public getAppRespResponseType getApplicationResponse(getAppRespRequestType instanceIdentifier)
        {
            try
            {
                var h = new H2oServiceRequester();
                LogWriter log = new LogWriter();
                log.Requestci(instanceIdentifier);

                getAppRespResponse appResponse = new getAppRespResponse();

                var ReturnOfService = h.CheckIncomingEnvelope(instanceIdentifier);
                getAppRespResponseType appRespResponse = JsonSerializer.Deserialize(ReturnOfService);
                if (appRespResponse.applicationResponse == "ZARF ID BULUNAMADI")
                {
                    throw new FaultException("2004");
                }
                return appResponse.getResponse(appRespResponse.applicationResponse);
            }
            catch(FaultException faultCode)
            {
                EFaturaFault faultResponse = new EFaturaFault();
                var fault = faultResponse.getResponse(Convert.ToInt32(faultCode)); 
                throw new FaultException<EFaturaFaultType>(fault);
            }
        }
    }

    [ServiceContract]
    public interface IFaturaService
    {
        [OperationContract]
        [FaultContract(typeof(EFaturaFaultType), Action = "http://tempuri.org/IFaturaService/getApplicationResponse", Name = "EFaturaFault")]
        getAppRespResponseType getApplicationResponse(getAppRespRequestType instanceIdentifier);

        [OperationContract]
        [FaultContract(typeof(EFaturaFaultType), Action = "http://tempuri.org/IFaturaService/sendDocument",Name ="EFaturaFault")]
        documentReturnType sendDocument(documentType document);
    }
}
