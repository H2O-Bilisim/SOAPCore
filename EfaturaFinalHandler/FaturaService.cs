using EfaturaFinalHandler.Models;
using System.ServiceModel;
using System.Threading;
namespace EfaturaFinalHandler
{
    public class FaturaService : EFatura
    {
        private ThreadLocal<string> _paramValue = new ThreadLocal<string>() { Value = string.Empty };

        public documentResponse sendDocument(documentRequest document)
        {
            LogWriter log = new LogWriter();
            log.Requestci(document);

            DocumentController documentController = new DocumentController();
            documentController.ValidateDocument(document);
            var response = new documentReturnType().getResponse();
            log.Responscu(response);
            return response;
        }
        public getAppRespResponse getApplicationResponse(getAppRespRequest instanceIdentifier)
        {
            LogWriter log = new LogWriter();
            log.Requestci(instanceIdentifier);
            var response = new getAppRespResponseType().getResponse(instanceIdentifier.instanceIdentifier);
            if (response.applicationResponse == "ZARF ID BULUNAMADI")
            {
                EFaturaFaultType fault = new EFaturaFaultType();
                fault.throwResponse("2004");
            }
            log.Responscu(response);
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
