using Efatura.Interfaces;
using Efatura.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml;

namespace Efatura.Repositories
{
    public class EfaturaRepository : IEfaturaRepository
    {

        private readonly IEfaturaChannel _proxy;

        public EfaturaRepository(IOptions<ClientConfig> config)
        {

            var cfg = config.Value;

            /*
             * Create & Configure Client
             */
            BasicHttpBinding binding = new BasicHttpBinding
            {
                SendTimeout = TimeSpan.FromSeconds(cfg.Timeout),
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                AllowCookies = true,
                ReaderQuotas = XmlDictionaryReaderQuotas.Max
            };
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            EndpointAddress address = new EndpointAddress(cfg.Url);
            ChannelFactory<IEfaturaChannel> factory = new ChannelFactory<IEfaturaChannel>(binding, address);
            this._proxy = factory.CreateChannel();

        }

        public async Task<string> FahrenheitToCelsiusAsync(string fahrenheit)
        {
            return await _proxy.FahrenheitToCelsiusAsync(fahrenheit);

        }

        public async Task<string> CelsiusToFahrenheitAsync(string celsius)
        {
            return await _proxy.CelsiusToFahrenheitAsync(celsius);
        }

    }
}
