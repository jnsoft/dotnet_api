using Microsoft.AspNetCore.Mvc;
using Models;
using myapi2.Authentication;

namespace myapi2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
       // [ServiceFilter(typeof(ApiKeyAuthFilter))] // method 3: using filters on controllers/methods
        public IEnumerable<WeatherForecast> Get() => Models.WeatherForecast.Generate();
        
    }
}