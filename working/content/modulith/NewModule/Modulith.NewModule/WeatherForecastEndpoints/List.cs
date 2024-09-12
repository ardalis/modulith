using FastEndpoints;
#if (withui)
using Modulith.NewModule.HttpModels;
#endif

namespace Modulith.NewModule.WeatherForecastEndpoints;

internal class List(IWeatherForecastService weatherForecastService) : EndpointWithoutRequest<IEnumerable<WeatherForecastResponse>>
{
  public override void Configure()
  {
    AllowAnonymous();
    Get("/NewModule/WeatherForecast");
  }

  public override async Task HandleAsync(CancellationToken ct) => await SendOkAsync(await weatherForecastService.GetWeatherForecastAsync(), ct);
}
