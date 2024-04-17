using FastEndpoints;
using static System.DateOnly;
using static System.Random;

namespace Modulith.NewModule;

internal record WeatherForecastResponse(DateOnly Date, int TemperatureC, string? Summary);

internal class WeatherForeCastEndpoint : EndpointWithoutRequest<WeatherForecastResponse[]>
{
  public override void Configure()
  {
    AllowAnonymous();
    Get("/NewModule/weatherforecast");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    string[] summaries =
      ["Freezing", "Bracing", "Chilly", "Cool", "Mild", 
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

    await SendOkAsync(Enumerable.Range(1, 5)
      .Select(random =>
        new WeatherForecastResponse
        (
          FromDateTime(DateTime.Now.AddDays(random)),
          Shared.Next(-20, 55),
          summaries[Shared.Next(summaries.Length)]
        ))
      .ToArray(), ct);
  }
}
