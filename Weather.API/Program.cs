using Weather.API.Endpoints;
using Weather.API.Interfaces;
using Weather.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IWeatherRepository, WeatherRepository>();
builder.Services.AddHttpClient<IWeatherRepository, WeatherRepository>(client =>
{
    var config = builder.Configuration;
    client.BaseAddress = new Uri(config.GetValue<string>("DBUrl")!);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapWeatherEndpoints();

app.Run();
