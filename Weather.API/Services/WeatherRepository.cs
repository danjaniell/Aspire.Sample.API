using System.Text.Json;
using Weather.API.Interfaces;
using Weather.API.Models;

namespace Weather.API.Services;

public class WeatherRepository(HttpClient httpClient) : IWeatherRepository
{
    public async Task<WeatherForecast> AddForecastAsync(WeatherForecast forecast)
    {
        var response = await httpClient.PostAsJsonAsync("/database", forecast);
        var result = await response.Content.ReadFromJsonAsync<WeatherForecast>();
        return result ?? throw new InvalidOperationException("Failed to add.");
    }

    public async Task<WeatherForecast> DeleteForecastAsync(int id)
    {
        var entity = await httpClient.GetFromJsonAsync<WeatherForecast>($"/database/{id}");
        if (entity is not null)
        {
            await httpClient.DeleteAsync($"/database/{id}");
        }
        return entity ?? throw new InvalidOperationException("Failed to delete.");
    }

    public async Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
    {
        var response = await httpClient.GetFromJsonAsync<WeatherForecast[]>(
            $"/database?startDate={startDate:yyyy-MM-dd}",
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        if (response is null)
        {
            return [];
        }

        return response;
    }

    public async Task<WeatherForecast> GetForecastAsync(int id)
    {
        var response = await httpClient.GetFromJsonAsync<WeatherForecast>($"/database/{id}");
        return response ?? throw new InvalidOperationException("Failed to get.");
    }

    public async Task<WeatherForecast> GetForecastByIdAsync(int id)
    {
        var response = await httpClient.GetFromJsonAsync<WeatherForecast>($"/database/{id}");
        return response ?? throw new InvalidOperationException("Failed to get.");
    }

    public Task<int> GetMaxId()
    {
        return httpClient.GetFromJsonAsync<int>("/database/maxId");
    }

    public async Task<WeatherForecast> UpdateForecastAsync(int id, WeatherForecast forecast)
    {
        var entity = await httpClient.GetFromJsonAsync<WeatherForecast>($"/database/{id}");
        if (entity is not null)
        {
            await httpClient.PutAsJsonAsync($"/database/{id}", forecast);
        }
        return forecast ?? throw new InvalidOperationException("Failed to update.");
    }
}
