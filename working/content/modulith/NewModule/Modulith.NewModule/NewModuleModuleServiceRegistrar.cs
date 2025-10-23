﻿#if (WithUi)
using Modulith.NewModule.HttpModels;
#endif
using Microsoft.Extensions.DependencyInjection;
using Modulith.SharedKernel;
using Microsoft.Extensions.Configuration;

namespace Modulith.NewModule;

public class NewModuleModuleServiceRegistrar : IRegisterModuleServices
{
  public static IServiceCollection ConfigureServices(IServiceCollection services,
    IConfiguration configuration)
  {
    services.AddScoped<IWeatherForecastService, ServerWeatherForecastService>();

    return services;
  }
}
