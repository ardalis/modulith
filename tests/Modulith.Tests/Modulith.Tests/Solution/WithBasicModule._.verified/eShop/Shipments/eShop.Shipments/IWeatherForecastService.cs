using eShop.Shipments.WeatherForecastEndpoints;
namespace eShop.Shipments;

internal interface IWeatherForecastService
{
  Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecastAsync();
}
