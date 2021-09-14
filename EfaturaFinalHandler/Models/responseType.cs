using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace EfaturaFinalHandler.Models
{
    /*
        Document Response Type Class
        Create object type document response data
        @return Object
            String $msg = Response message
            String $hash = MD5 hash of msg variable
    */
    [DataContract]
    public class DocumentResponseType
    {
        [DataMember]
        public string msg { get; set; }
        [DataMember]
        public string hash { get; set; }
    }

    public class DocumentResponse
    {
        private DocumentResponseType _getDocumentResponse;
        public DocumentResponse()
        {
            _getDocumentResponse = new DocumentResponseType();
        }
        public DocumentResponseType getResponse(int code)
        {
            switch (code)
            {
                case 0:
                    _getDocumentResponse.msg = "ZARF BASARIYLA ALINDI";
                    _getDocumentResponse.hash = "c6fa444a15bf9cc2d1b62350e5a8bf39";
                    break;
                case 1:
                    _getDocumentResponse.msg = "ZARF KUYRUGA EKLENDI";
                    _getDocumentResponse.hash = "b2721454481f0672ce3a7ee41c1c101c";
                    break;
                default:
            }
            return _getDocumentResponse;
        }
    }
    /*
        END Document Response Type Class
    */
    /***************************************************************************/
    /*
        Fault Response Type Class
        Create object type fault response data
        @return Object
            Integer $code = Fault code
            String $msg = Fault message
    */
    [DataContract]
    public class FaultResponseType
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]      
        public string msg { get; set; }  
    }

    public class FaultResponse
    {
        private FaultResponseType _getFaultResponse;
        public FaultResponse()
        {
            _getFaultResponse = new FaultResponseType();
        }
        public FaultResponseType getResponse(int code = null)
        {
            switch (code)
            {
                case 2000:
                    _getFaultResponse.code = code;
                    _getFaultResponse.msg = "OZET DEGERLER ESIT DEGIL";
                    break;
                case 2001:
                    _getFaultResponse.code = code;
                    _getFaultResponse.msg = "ZARF ID SISTEMDE MEVCUT";
                    break;
                case 2003:
                    _getFaultResponse.code = code;
                    _getFaultResponse.msg = "ZARF KUYRUGA EKLENEMEDI";
                    break;
                case 2004:
                    _getFaultResponse.code = code;
                    _getFaultResponse.msg = "ZARF ID BULUNAMADI";
                    break;
                case 2006:
                    _getFaultResponse.code = code;
                    _getFaultResponse.msg = "GECERSIZ ZARF ADI";
                    break;
                default:
                    _getFaultResponse.code = 2003;
                    _getFaultResponse.msg = "ZARF KUYRUGA EKLENEMEDI";
            }
            return _getFaultResponse;
        }
    }
    /*
        END Fault Response Type Class
    */
    /***************************************************************************/
    /*
        AppResp Response Type Class
        Create object type AppResp response data
        @return Object
            String $applicationResponse = Envelope Status message
    */
    [DataContract]
    public class AppRespResponseType
    {
        [DataMember]
        public string applicationResponse { get; set; }
    }

    public class AppRespResponse
    {
        private AppRespResponseType _getAppRespResponse;
        public AppRespResponse()
        {
            _getAppRespResponse = new AppRespResponseType();
        }
        public AppRespResponseType getResponse(string status)
        {
            _getAppRespResponse.applicationResponse = status;
            return _getAppRespResponse;
        }
    }
    /*
        END AppResp Response Type Class
    */
}
