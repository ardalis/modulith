namespace Modulith.NewModule.Api;

internal interface IWeatherForecastService
{
  WeatherForecastResponse[] GetWeatherForecast(string[] summaries);
}
