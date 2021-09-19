﻿using System;
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
            getAppRespRequest request = new getAppRespRequest();
            request.instanceIdentifier = instanceIdentifier;

            var reqObj = new H2oServiceRequester();
            var service = reqObj.CheckIncomingEnvelope(request);

            _getAppRespResponse.applicationResponse = service.applicationResponse;
            if (_getAppRespResponse.applicationResponse == "ZARF ID BULUNAMADI")
            {
                EFaturaFaultType fault = new EFaturaFaultType();
                fault.throwResponse("2004");
            }
            return _getAppRespResponse;
        }
    }
}
