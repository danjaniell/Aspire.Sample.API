using Weather.API.Interfaces;
using Weather.API.Models;

namespace Weather.API.Endpoints;

public static class WeatherEndpoints
{
    public static void MapWeatherEndpoints(this WebApplication app)
    {
        app.MapGet(
            "/weatherforecast",
            async (IWeatherRepository weatherRepository) =>
            {
                var forecasts = await weatherRepository.GetForecastAsync(DateTime.Now);
                return Results.Ok(forecasts);
            }
        );

        app.MapGet(
            "/weatherforecast/{id}",
            async (int id, IWeatherRepository weatherRepository) =>
            {
                var forecast = await weatherRepository.GetForecastByIdAsync(id);
                if (forecast is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(forecast);
            }
        );

        app.MapPost(
            "/weatherforecast",
            async (WeatherForecast forecast, IWeatherRepository weatherRepository) =>
            {
                var result = await weatherRepository.AddForecastAsync(forecast);
                var maxId = await weatherRepository.GetMaxId();
                return Results.Created($"/weatherforecast/{maxId + 1}", result);
            }
        );

        app.MapPut(
            "/weatherforecast/{id}",
            async (int id, WeatherForecast forecast, IWeatherRepository weatherRepository) =>
            {
                var result = await weatherRepository.UpdateForecastAsync(id, forecast);
                if (result is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(result);
            }
        );

        app.MapDelete(
            "/weatherforecast/{id}",
            async (int id, IWeatherRepository weatherRepository) =>
            {
                var result = await weatherRepository.DeleteForecastAsync(id);
                if (result is null)
                {
                    return Results.NotFound();
                }
                return Results.NoContent();
            }
        );
    }
}
