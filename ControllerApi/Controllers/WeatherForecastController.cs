using Microsoft.AspNetCore.Mvc;
using ControllerApi.Models;
using ControllerApi.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace ControllerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))] //method 2b: using filters on controllers (or methods in controller)
    public class WeatherForecastController : ControllerBase
    {
        

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        // [ServiceFilter(typeof(ApiKeyAuthFilter))]
        public IEnumerable<WeatherForecast> Get() => Models.WeatherForecast.Generate();

       

    }
}