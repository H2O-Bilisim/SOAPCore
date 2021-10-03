using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfaturaFinalHandler.Models
{
    public class getAppRespResponseType
    {
        private getAppRespResponse _getAppRespResponse;
        public getAppRespResponseType() => _getAppRespResponse = new getAppRespResponse();
        public getAppRespResponse getResponse(string instanceIdentifier)
        {
            var reqObj = new H2oServiceRequester();
            
            getAppRespRequest request = new getAppRespRequest();
            request.instanceIdentifier = instanceIdentifier;
            
            var service = reqObj.CheckIncomingEnvelope(request);

            _getAppRespResponse.applicationResponse = service.applicationResponse;
           
            return _getAppRespResponse;
        }
    }
}
