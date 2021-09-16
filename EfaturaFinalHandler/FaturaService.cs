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
            int validCode = documentController.ValidateDocument(document);

            return new documentReturnType().getResponse(validCode);
        }
        public getAppRespResponse getApplicationResponse(getAppRespRequest instanceIdentifier)
        {
            LogWriter log = new LogWriter();
            log.Requestci(instanceIdentifier);

            return new getAppRespResponseType().getResponse(instanceIdentifier.instanceIdentifier);
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
