using FastEndpoints;

namespace eShop.Shipments;

internal record WeatherForecastResponse(DateOnly Date, int TemperatureC, string? Summary);
internal class WeatherForecastEndpoint(IWeatherForecastService weatherForecastService) : EndpointWithoutRequest<IEnumerable<WeatherForecastResponse>>
{
  public override void Configure()
  {
    AllowAnonymous();
    Get("/Shipments/weatherforecast");
  }

  public override async Task HandleAsync(CancellationToken ct) => await SendOkAsync(await weatherForecastService.GetWeatherForecastAsync(), ct);
}
