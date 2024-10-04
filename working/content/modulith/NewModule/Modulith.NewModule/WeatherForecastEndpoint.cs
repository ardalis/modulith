using FastEndpoints;
#if (WithUi)
using Modulith.NewModule.HttpModels;
#endif

namespace Modulith.NewModule;

#if (!WithUi)
internal record WeatherForecastResponse(DateOnly Date, int TemperatureC, string? Summary);
#endif
internal class WeatherForecastEndpoint(IWeatherForecastService weatherForecastService) : EndpointWithoutRequest<IEnumerable<WeatherForecastResponse>>
{
  public override void Configure()
  {
    AllowAnonymous();
    Get("/NewModule/weatherforecast");
  }

  public override async Task HandleAsync(CancellationToken ct) => await SendOkAsync(await weatherForecastService.GetWeatherForecastAsync(), ct);
}
