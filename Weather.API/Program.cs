using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
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
    client.BaseAddress = new Uri(
        Environment.GetEnvironmentVariable("DBUrl")
            ?? config.GetValue<string>("DBUrl")
            ?? "http://localhost:7227"
    );
});

builder
    .Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .AddRuntimeInstrumentation()
            .AddMeter(
                "Microsoft.AspNetCore.Hosting",
                "Microsoft.AspNetCore.Server.Kestrel",
                "System.Net.Http"
            );
    })
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation();
    });

var useOtlpExporter = !string.IsNullOrWhiteSpace(
    builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]
);
if (useOtlpExporter)
{
    builder.Services.AddOpenTelemetry().UseOtlpExporter();
}

var app = builder.Build();

Console.WriteLine($"HttpClient BaseAddress: {app.Services.GetService<HttpClient>().BaseAddress}");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapWeatherEndpoints();

app.Run();
