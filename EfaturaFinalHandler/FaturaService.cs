using EfaturaFinalHandler.Logger;
using EfaturaFinalHandler.Models;
using System.ServiceModel;
using System.Threading;


namespace EfaturaFinalHandler
{
    public class FaturaService : EFatura
    {
        private ThreadLocal<string> _paramValue = new ThreadLocal<string>() { Value = string.Empty };
        public static Serilog.Core.Logger Log;

        public FaturaService()
        {
            // Get Logger
            var Logger = new SeriLogger();
            Log = Logger.Log;
        }

        public documentResponse sendDocument(documentRequest document)
        {
            LogWriter log = new LogWriter();
            log.Requestci(document);
            Log.Information("{Module} received a request | ID: {DocID} RequestObject: {SendDocRequest}", "sendDocument", document.fileName, document);

            DocumentController documentController = new DocumentController();
            documentController.ValidateDocument(document);
            var response = new documentReturnType().getResponse();

            log.Responscu(response);
            Log.Information("{Module] is sending response for document {RequestedDocID} | ResponseObject: {SendDocResponse}", "sendDocument", document.fileName, response);
            
            return response;
        }
        public getAppRespResponse getApplicationResponse(getAppRespRequest instanceIdentifier)
        {
            LogWriter log = new LogWriter();
            log.Requestci(instanceIdentifier);
            Log.Information("{Module} received a request | ID: {DocID}", "getApplicationResponse", instanceIdentifier);

            var response = new getAppRespResponseType().getResponse(instanceIdentifier.instanceIdentifier);
            if (response.applicationResponse == "ZARF ID BULUNAMADI")
            {
                EFaturaFaultType fault = new EFaturaFaultType();
                fault.throwResponse("2004");
            }

            log.Responscu(response);
            Log.Information("{Module] id sending response for requested doc {RequestedDocId} | ResponseObject: {GetAppRespResponse}", "sendDocument", instanceIdentifier, response);

            return response;
        }
    }

    [ServiceContract]
    public interface EFatura
    {
        [OperationContract]
        [FaultContract(typeof(EFaturaFault),Name ="EFatura")]
        getAppRespResponse getApplicationResponse(getAppRespRequest instanceIdentifier);

        [OperationContract]
        [FaultContract(typeof(EFaturaFault), Name = "EFatura")]
        documentResponse sendDocument(documentRequest document);
    }
}
