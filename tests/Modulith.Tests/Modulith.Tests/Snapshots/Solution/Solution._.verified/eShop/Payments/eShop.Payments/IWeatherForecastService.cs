using eShop.Payments.WeatherForecastEndpoints;
namespace eShop.Payments;

internal interface IWeatherForecastService
{
  Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecastAsync();
}
