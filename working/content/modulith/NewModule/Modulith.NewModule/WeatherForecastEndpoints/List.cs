using FastEndpoints;
using MediatR;
#if (withui)
using Modulith.NewModule.HttpModels;
#endif

namespace Modulith.NewModule.WeatherForecastEndpoints;

internal class List(ISender sender) : EndpointWithoutRequest<IEnumerable<WeatherForecastResponse>>
{
  public override void Configure()
  {
    AllowAnonymous();
    Get("/NewModule/WeatherForecast");
  }

  public override async Task HandleAsync(CancellationToken ct) => await SendOkAsync(await sender.Send(new WeatherForecastQuery(), ct), ct);
}
