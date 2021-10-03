using EfaturaFinalHandler.Helpers;

namespace EfaturaFinalHandler.Models
{
    public class documentReturnType
    {
        private documentResponse _getDocumentResponse;
        private CryptoHelpers _ch;
        public documentReturnType()
        {
            _getDocumentResponse = new documentResponse();
            _ch = new CryptoHelpers();
        }
        public documentResponse getResponse()
        {
            _getDocumentResponse.msg = "ZARF KUYRUGA EKLENDI";
            _getDocumentResponse.hash = _ch.GetMd5Hash(_getDocumentResponse.msg);
            return _getDocumentResponse;
        }
    }
    /*
        END Fault Response Type Class
    */
}
