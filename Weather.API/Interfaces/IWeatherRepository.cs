using Weather.API.Models;

namespace Weather.API.Interfaces;

public interface IWeatherRepository
{
    Task<WeatherForecast[]> GetForecastAsync(DateTime startDate);

    Task<WeatherForecast> GetForecastAsync(int id);

    Task<WeatherForecast> AddForecastAsync(WeatherForecast forecast);

    Task<WeatherForecast> UpdateForecastAsync(int id, WeatherForecast forecast);

    Task<WeatherForecast> DeleteForecastAsync(int id);

    Task<WeatherForecast> GetForecastByIdAsync(int id);
    Task<int> GetMaxId();
}
