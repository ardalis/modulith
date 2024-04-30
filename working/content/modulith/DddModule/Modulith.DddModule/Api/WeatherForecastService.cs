namespace Modulith.DddModule.Api;

internal class WeatherForecastService : IWeatherForecastService
{
  public WeatherForecastResponse[] GetWeatherForecast(string[] summaries)
  {

    return Enumerable.Range(1, 5)
      .Select(random =>
        new WeatherForecastResponse
        (
          DateOnly.FromDateTime(DateTime.Now.AddDays(random)),
          Random.Shared.Next(-20, 55),
          summaries[Random.Shared.Next(summaries.Length)]
        ))
      .ToArray();
  }
}
