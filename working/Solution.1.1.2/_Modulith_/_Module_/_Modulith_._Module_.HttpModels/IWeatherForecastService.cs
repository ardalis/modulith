namespace _Modulith_._Module_.HttpModels;

public interface IWeatherForecastService
{
  Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecastAsync();
}
