namespace Modulith.NewModule.HttpModels;

public record WeatherForecastResponse(DateOnly Date, int TemperatureC, string? Summary)
{
    public int      TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

