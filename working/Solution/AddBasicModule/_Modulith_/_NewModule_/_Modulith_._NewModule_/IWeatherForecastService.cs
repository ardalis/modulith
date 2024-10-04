using _Modulith_._NewModule_.WeatherForecastEndpoints;
namespace _Modulith_._NewModule_;

internal interface IWeatherForecastService
{
  Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecastAsync();
}
