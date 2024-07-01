using Modulith.NewModule.HttpModels;

namespace Modulith.NewModule.HttpModels;

public interface IWeatherForecastService
{
  Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecastAsync();
}
