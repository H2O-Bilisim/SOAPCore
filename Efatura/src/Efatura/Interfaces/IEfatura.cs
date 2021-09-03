using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Efatura.Interfaces
{
    [System.ServiceModel.ServiceContractAttribute(Namespace = "https://www.w3schools.com/xml/", ConfigurationName = "Efatura.Services.Interfaces.IEfatura")]
    public interface IEfatura
    {

        [System.ServiceModel.OperationContractAttribute(Action = "https://www.w3schools.com/xml/FahrenheitToCelsius", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        Task<string> FahrenheitToCelsiusAsync(string Fahrenheit);

        [System.ServiceModel.OperationContractAttribute(Action = "https://www.w3schools.com/xml/CelsiusToFahrenheit", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        Task<string> CelsiusToFahrenheitAsync(string Celsius);
    }
}
