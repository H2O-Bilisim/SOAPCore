using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Efatura.Interfaces
{
    public interface IEfaturaRepository
    {
        Task<string> FahrenheitToCelsiusAsync(string fahrenheit);

        Task<string> CelsiusToFahrenheitAsync(string celsius);
    }
}
