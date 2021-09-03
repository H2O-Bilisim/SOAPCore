using Efatura.Interfaces;
using Efatura.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Efatura.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EFaturaController : ControllerBase
    {
        private readonly IEfaturaRepository _temp;

        public EFaturaController(IEfaturaRepository temp)
        {
            _temp = temp;
        }

        [HttpPost("celsius")]
        public async Task<IActionResult> Celsius(ConvertRequest model)
        {
            var res = await _temp.FahrenheitToCelsiusAsync(model.Value);

            return Ok(new
            {
                Success = true,
                Temp = $"{model.Value} degrees fahrenheit is == to {res} degrees celsius"
            });
        }

        [HttpPost("fahrenheit")]
        public async Task<IActionResult> Fahrenheit(ConvertRequest model)
        {
            var res = await _temp.CelsiusToFahrenheitAsync(model.Value);

            return Ok(new
            {
                Success = true,
                Temp = $"{model.Value} degrees celsius is == to {res} degrees fahrenheit"
            });
        }
    }
}
