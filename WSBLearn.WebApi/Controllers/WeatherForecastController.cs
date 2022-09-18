using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Services;

namespace WSBLearn.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _forecastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService forecastService)
        {
            _logger = logger;
            _forecastService = forecastService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult GetAll()
        {
            _logger.LogWarning("This is a test of logger!");
            return Ok(_forecastService.Get());
        }
    }
}