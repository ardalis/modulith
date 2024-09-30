using System.Text.Json;
using eShop.Payments.HttpModels;

namespace eShop.Payments.UI;

public class ClientWeatherForecastService(HttpClient client) : IWeatherForecastService
{
  public async Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecastAsync()
  {
    var response       = await client.GetAsync("Payments/weatherforecast");
    var stringResponse = await response.Content.ReadAsStringAsync();
    var forecasts = JsonSerializer.Deserialize<List<WeatherForecastResponse>>(stringResponse, new JsonSerializerOptions()
    {
      PropertyNameCaseInsensitive = true
    });

    return forecasts ?? [];
  }
}
