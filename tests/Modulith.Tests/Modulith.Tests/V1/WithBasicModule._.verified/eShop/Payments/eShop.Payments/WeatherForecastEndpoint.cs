using FastEndpoints;

namespace eShop.Payments;

internal record WeatherForecastResponse(DateOnly Date, int TemperatureC, string? Summary);
internal class WeatherForecastEndpoint(IWeatherForecastService weatherForecastService) : EndpointWithoutRequest<IEnumerable<WeatherForecastResponse>>
{
  public override void Configure()
  {
    AllowAnonymous();
    Get("/Payments/weatherforecast");
  }

  public override async Task HandleAsync(CancellationToken ct) => await SendOkAsync(await weatherForecastService.GetWeatherForecastAsync(), ct);
}
