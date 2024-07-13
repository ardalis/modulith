namespace Modulith.DddModule.Api;

internal interface IWeatherForecastService
{
  WeatherForecastResponse[] GetWeatherForecast(string[] summaries);
}
