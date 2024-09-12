#if (withui)
using Modulith.NewModule.HttpModels;
#else
using Modulith.NewModule.WeatherForecastEndpoints;
#endif
namespace Modulith.NewModule;

internal interface IWeatherForecastService
{
  Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecastAsync();
}
