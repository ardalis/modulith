#if (WithUi)
using Modulith.NewModule.HttpModels;
#endif
namespace Modulith.NewModule;

public interface IWeatherForecastService
{
  Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecastAsync();
}
