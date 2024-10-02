namespace _Modulith_.DddModule.Api;

internal interface IWeatherForecastService
{
  WeatherForecastResponse[] GetWeatherForecast(string[] summaries);
}
