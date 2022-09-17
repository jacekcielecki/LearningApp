using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Services
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> Get();
    }
}