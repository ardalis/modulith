namespace _Modulith_.NewModule.WeatherForecastEndpoints;

internal record WeatherForecastResponse(DateOnly Date, int TemperatureC, string? Summary);