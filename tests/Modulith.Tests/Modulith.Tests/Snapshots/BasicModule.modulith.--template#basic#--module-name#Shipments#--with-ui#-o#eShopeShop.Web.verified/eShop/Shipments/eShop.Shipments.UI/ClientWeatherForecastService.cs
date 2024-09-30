using System.Text.Json;
using eShop.Shipments.HttpModels;

namespace eShop.Shipments.UI;

public class ClientWeatherForecastService(HttpClient client) : IWeatherForecastService
{
  public async Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecastAsync()
  {
    var response       = await client.GetAsync("Shipments/weatherforecast");
    var stringResponse = await response.Content.ReadAsStringAsync();
    var forecasts = JsonSerializer.Deserialize<List<WeatherForecastResponse>>(stringResponse, new JsonSerializerOptions()
    {
      PropertyNameCaseInsensitive = true
    });

    return forecasts ?? [];
  }
}
