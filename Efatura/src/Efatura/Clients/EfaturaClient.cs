using Efatura.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using System.Xml;

namespace Efatura.Clients
{
    public class EfaturaClient : ClientBase<IEfatura>, IEfatura
    {
        static void ConfigureEndpoint(ServiceEndpoint serviceEndpoint, ClientCredentials clientCredentials)
        {
            throw new NotImplementedException();
        }

        public EfaturaClient(EndpointConfiguration endpointConfiguration) :
            base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
        {
            Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(Endpoint, ClientCredentials);
        }

        public EfaturaClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
            base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
        {
            Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(Endpoint, ClientCredentials);
        }

        public EfaturaClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress) :
            base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(Endpoint, ClientCredentials);
        }

        public EfaturaClient(Binding binding, EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public Task<string> FahrenheitToCelsiusAsync(string Fahrenheit)
        {
            return Channel.FahrenheitToCelsiusAsync(Fahrenheit);
        }

        public Task<string> CelsiusToFahrenheitAsync(string Celsius)
        {
            return Channel.CelsiusToFahrenheitAsync(Celsius);
        }

        public virtual Task OpenAsync()
        {
            return Task.Factory.FromAsync(((ICommunicationObject)(this)).BeginOpen(null, null), ((ICommunicationObject)(this)).EndOpen);
        }

        public virtual Task CloseAsync()
        {
            return Task.Factory.FromAsync(((ICommunicationObject)(this)).BeginClose(null, null), ((ICommunicationObject)(this)).EndClose);
        }

        private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.EfaturaSoap))
            {
                BasicHttpBinding result = new BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.EfaturaSoap12))
            {
                CustomBinding result = new CustomBinding();
                TextMessageEncodingBindingElement textBindingElement = new TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                HttpTransportBindingElement httpBindingElement = new HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }

        private static EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.EfaturaSoap))
            {
                return new EndpointAddress("http://www.w3schools.com/xml/tempconvert.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.EfaturaSoap12))
            {
                return new EndpointAddress("http://www.w3schools.com/xml/tempconvert.asmx");
            }
            throw new InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }

        public enum EndpointConfiguration
        {

            EfaturaSoap,

            EfaturaSoap12
        }
    }
}
