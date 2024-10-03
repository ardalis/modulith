using FastEndpoints;
using MediatR;
using _Modulith_._BlazorModule_.HttpModels;

namespace _Modulith_._BlazorModule_.WeatherForecastEndpoints;

internal class List(ISender sender) : EndpointWithoutRequest<IEnumerable<WeatherForecastResponse>>
{
  public override void Configure()
  {
    AllowAnonymous();
    Get("/_BlazorModule_/WeatherForecast");
  }

  public override async Task HandleAsync(CancellationToken ct) => await SendOkAsync(await sender.Send(new WeatherForecastQuery(), ct), ct);
}
