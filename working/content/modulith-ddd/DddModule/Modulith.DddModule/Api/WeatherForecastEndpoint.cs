using FastEndpoints;

namespace Modulith.DddModule.Api;

internal record WeatherForecastResponse(DateOnly Date, int TemperatureC, string? Summary);

internal class WeatherForecastEndpoint(IWeatherForecastService weatherForecastService) : EndpointWithoutRequest<WeatherForecastResponse[]>
{
  public override void Configure()
  {
    AllowAnonymous();
    Get("/DddModule/weatherforecast");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    string[] summaries =
      ["Freezing", "Bracing", "Chilly", "Cool", "Mild", 
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

    var forecasts = weatherForecastService.GetWeatherForecast(summaries);
    
    await SendOkAsync(forecasts, ct);
  }
}
