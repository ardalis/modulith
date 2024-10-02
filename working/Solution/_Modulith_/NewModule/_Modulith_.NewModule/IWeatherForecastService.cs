#if (withui)
using _Modulith_.NewModule.HttpModels;
#else
using _Modulith_.NewModule.WeatherForecastEndpoints;
#endif
namespace _Modulith_.NewModule;

internal interface IWeatherForecastService
{
  Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecastAsync();
}
