using static System.DateOnly;
using static System.Random;

namespace eShop.Payments;

internal class ServerWeatherForecastService : IWeatherForecastService
{
  public Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecastAsync()
  {
    string[] summaries =
    [
      "Freezing", "Bracing", "Chilly", "Cool", "Mild",
      "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    return Task.FromResult(Enumerable.Range(1, 5)
      .Select(random =>
        new WeatherForecastResponse
        (
          FromDateTime(DateTime.Now.AddDays(random)),
          Shared.Next(-20, 55),
          summaries[Shared.Next(summaries.Length)]
        )));
  }
}
