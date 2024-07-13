using System.Text.Json;
using Modulith.NewModule.HttpModels;

namespace Modulith.NewModule.UI;

public class ClientWeatherForecastService(HttpClient client) : IWeatherForecastService
{
  public async Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecastAsync()
  {
    var response       = await client.GetAsync("NewModule/weatherforecast");
    var stringResponse = await response.Content.ReadAsStringAsync();
    var forecasts = JsonSerializer.Deserialize<List<WeatherForecastResponse>>(stringResponse, new JsonSerializerOptions()
    {
      PropertyNameCaseInsensitive = true
    });

    return forecasts ?? [];
  }
}
