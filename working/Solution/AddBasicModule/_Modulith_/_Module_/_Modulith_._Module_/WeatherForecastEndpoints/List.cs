using FastEndpoints;
using MediatR;

namespace _Modulith_._Module_.WeatherForecastEndpoints;

internal class List(ISender sender) : EndpointWithoutRequest<IEnumerable<WeatherForecastResponse>>
{
  public override void Configure()
  {
    AllowAnonymous();
    Get("/_Module_/WeatherForecast");
  }

  public override async Task HandleAsync(CancellationToken ct) => await SendOkAsync(await sender.Send(new WeatherForecastQuery(), ct), ct);
}
