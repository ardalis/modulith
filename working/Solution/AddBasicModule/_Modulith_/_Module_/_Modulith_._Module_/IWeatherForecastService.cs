﻿using _Modulith_._Module_.WeatherForecastEndpoints;
namespace _Modulith_._Module_;

internal interface IWeatherForecastService
{
  Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecastAsync();
}
