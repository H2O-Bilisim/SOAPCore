using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace EfaturaFinalHandler.Models
{
    public class EFaturaFaultType
    {
        private EFaturaFault _getFaultResponse;
        public EFaturaFaultType() { 
            _getFaultResponse = new EFaturaFault();
            _getFaultResponse.faultTypes = new Dictionary<FaultCode, FaultReason>();
            _getFaultResponse.faultTypes.Add(new FaultCode("2000"),new FaultReason(new FaultReasonText("OZET DEGERLER ESIT DEGIL")));
            _getFaultResponse.faultTypes.Add(new FaultCode("2001"), new FaultReason(new FaultReasonText("ZARF ID SISTEMDE MEVCUT")));
            _getFaultResponse.faultTypes.Add(new FaultCode("2002"), new FaultReason(new FaultReasonText("ZARF ARSIVE EKLENEMEDI")));
            _getFaultResponse.faultTypes.Add(new FaultCode("2003"), new FaultReason(new FaultReasonText("ZARF KUYRUGA EKLENEMEDI")));
            _getFaultResponse.faultTypes.Add(new FaultCode("2004"), new FaultReason(new FaultReasonText("ZARF ID BULUNAMADI")));
            _getFaultResponse.faultTypes.Add(new FaultCode("2005"), new FaultReason(new FaultReasonText("SISTEM HATASI")));
            _getFaultResponse.faultTypes.Add(new FaultCode("2006"), new FaultReason(new FaultReasonText("GECERSIZ ZARF ADI")));
            _getFaultResponse.faultTypes.Add(new FaultCode("2007"), new FaultReason(new FaultReasonText("PAKET   GÖNDERMEYE   VE   SORGULAMAYA   YETKİNİZ   GEÇİCİ   OLARAK KALDIRILMIŞTIR.")));
        }
        public void throwResponse(string strCode)
        {
            FaultCode faultCode = new FaultCode("2005");
            FaultReason faultReason = new FaultReason(new FaultReasonText("SISTEM HATASI"));
            if (string.IsNullOrEmpty(strCode))
            {
                _getFaultResponse.code = 2000;
                foreach (var item in _getFaultResponse.faultTypes)
                {
                    if (item.Key.Name == "2000")
                    {
                        faultReason = item.Value;
                        faultCode = new FaultCode("2000");
                        _getFaultResponse.msg = item.Value.ToString();
                        break;
                    }
                }
            }
            else
            {
                _getFaultResponse.code = Convert.ToInt32(strCode);
                foreach(var item in _getFaultResponse.faultTypes)
                {
                    if(item.Key.Name == strCode)
                    {
                        faultReason = item.Value;
                        faultCode = new FaultCode(strCode);
                        _getFaultResponse.msg = item.Value.ToString();
                        break;
                    }
                }
            }
            LogWriter log = new LogWriter();
            log.Responscu(_getFaultResponse);
            throw new FaultException<EFaturaFault>(_getFaultResponse, reason: faultReason, code:faultCode, strCode);
        }
    }
}
