// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Runtime.Serialization;
// using System.Threading.Tasks;

// namespace EfaturaFinalHandler.Models
// {
//     [DataContract]
//     public class FaultResponseType
//     {
//         [DataMember]
//         public int code { get; set; }
//         [DataMember]      
//         public string msg { get; set; }  
//     }

//     public class FaultResponse
//     {
//         private faultResponseType _getFaultResponse;
//         public FaultResponse()
//         {
//             _getFaultResponse = new faultResponseType();
//         }
//         public faultResponseType getResponse(int code)
//         {
//             switch (code)
//             {
//                 case 2000:
//                     _getFaultResponse.code = code;
//                     _getFaultResponse.msg = "OZET DEGERLER ESIT DEGIL";
//                     break;
//                 case 2001:
//                     _getFaultResponse.code = code;
//                     _getFaultResponse.msg = "ZARF ID SISTEMDE MEVCUT";
//                     break;
//                 case 2003:
//                     _getFaultResponse.code = code;
//                     _getFaultResponse.msg = "ZARF KUYRUGA EKLENEMEDI";
//                     break;
//                 case 2004:
//                     _getFaultResponse.code = code;
//                     _getFaultResponse.msg = "ZARF ID BULUNAMADI";
//                     break;
//                 case 2006:
//                     _getFaultResponse.code = code;
//                     _getFaultResponse.msg = "GECERSIZ ZARF ADI";
//                     break;
//                 default:
//                     _getFaultResponse.code = 2003;
//                     _getFaultResponse.msg = "ZARF KUYRUGA EKLENEMEDI";
//             }
//             return _getFaultResponse;
//         }
//     }
// }
