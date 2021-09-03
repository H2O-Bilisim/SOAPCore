using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SoapCore.Extensibility;

namespace EfaturaFinalHandler
{
    public class EfaturaServiceOperationTuner : IServiceOperationTuner
    {
        public void Tune(HttpContext httpContext, object serviceInstance, SoapCore.ServiceModel.OperationDescription operation)
        {
            if (operation.Name.Equals("SomeOperationName"))
            {
                FaturaService service = serviceInstance as FaturaService;
                string result = string.Empty;
                Console.WriteLine(httpContext.Request);

                
            }
        }
    }
}
