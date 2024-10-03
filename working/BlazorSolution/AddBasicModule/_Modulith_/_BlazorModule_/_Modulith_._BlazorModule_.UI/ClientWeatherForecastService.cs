using System.Text.Json;
using _Modulith_._BlazorModule_.HttpModels;

namespace _Modulith_._BlazorModule_.UI;

public class ClientWeatherForecastService(HttpClient client) : IWeatherForecastService
{
  public async Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecastAsync()
  {
    var response       = await client.GetAsync("_BlazorModule_/weatherforecast");
    var stringResponse = await response.Content.ReadAsStringAsync();
    var forecasts = JsonSerializer.Deserialize<List<WeatherForecastResponse>>(stringResponse, new JsonSerializerOptions()
    {
      PropertyNameCaseInsensitive = true
    });

    return forecasts ?? [];
  }
}
