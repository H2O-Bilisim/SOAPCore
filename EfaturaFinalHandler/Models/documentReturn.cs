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
        public documentResponse getResponse(int code)
        {
            try
            {
                switch (code)
                {
                    case 0:
                        _getDocumentResponse.msg = "ZARF BASARIYLA ALINDI";
                        _getDocumentResponse.hash = _ch.GetMd5Hash(_getDocumentResponse.msg);
                        break;
                    case 1:
                        _getDocumentResponse.msg = "ZARF KUYRUGA EKLENDI";
                        _getDocumentResponse.hash = _ch.GetMd5Hash(_getDocumentResponse.msg);
                        break;
                }
            }
            catch
            {
                EFaturaFaultType faultResponse = new EFaturaFaultType();
                faultResponse.throwResponse("2005");
            }
            return _getDocumentResponse;
        }
    }
    /*
        END Fault Response Type Class
    */
}
