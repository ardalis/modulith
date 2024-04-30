using Modulith.DddModule.Api;

namespace Modulith.DddModule.Domain;

internal class Weather(DateOnly date, int temperatureC, Summary summary)
{
  public DateOnly Date         { get; init; } = date;
  public int      TemperatureC { get; init; } = temperatureC;
  public Summary  Summary      { get; init; } = summary;
}

// ⚠️ Don't do it 🙃.
// The class below references the infra project.
// Uncommenting the Weather class will make the test:
// DomainTypesShould.NotDependOnApiTypes fail. 

// internal class BadWeatherClass
// {
//     private readonly WeatherForecastResponse _response;
//
//     internal BadWeatherClass(WeatherForecastResponse response)
//     {
//         _response = response;
//     }
// }
