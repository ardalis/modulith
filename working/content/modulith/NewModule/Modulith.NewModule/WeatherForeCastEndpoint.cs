using FastEndpoints;
using Modulith.NewModule.HttpModels;

namespace Modulith.NewModule;
#if (!WithUi)
internal record WeatherForecastResponse(DateOnly Date, int TemperatureC, string? Summary);
#endif
internal class WeatherForeCastEndpoint(IWeatherForecastService weatherForecastService) : EndpointWithoutRequest<IEnumerable<WeatherForecastResponse>>
{
  public override void Configure()
  {
    AllowAnonymous();
    Get("/NewModule/weatherforecast");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    await SendOkAsync(await weatherForecastService.GetWeatherForecastAsync(), ct);
  }
}