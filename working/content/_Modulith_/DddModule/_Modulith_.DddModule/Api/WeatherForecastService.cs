using _Modulith_.DddModule.Infrastructure;

namespace _Modulith_.DddModule.Api;

internal class WeatherForecastService(
  ITemperatureService
    temperatureService) : IWeatherForecastService
{
  public WeatherForecastResponse[] GetWeatherForecast(string[] summaries) => Enumerable.Range(1, 5)
    .Select(random =>
      new WeatherForecastResponse
      (
        DateOnly.FromDateTime(DateTime.Now.AddDays(random)),
        temperatureService.GetTemperature(),
        summaries[Random.Shared.Next(summaries.Length)]
      ))
    .ToArray();
}
