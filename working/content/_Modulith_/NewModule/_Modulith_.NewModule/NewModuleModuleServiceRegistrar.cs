﻿#if (withui)
using _Modulith_.NewModule.HttpModels;
#endif
using Microsoft.Extensions.DependencyInjection;
using _Modulith_.SharedKernel;

namespace _Modulith_.NewModule;

public class NewModuleModuleServiceRegistrar : IRegisterModuleServices
{
  public static IServiceCollection ConfigureServices(IServiceCollection services)
  {
    services.AddMediatR(
      c => c.RegisterServicesFromAssemblies(typeof(AssemblyInfo).Assembly));

    services.AddScoped<IWeatherForecastService, ServerWeatherForecastService>();

    return services;
  }
}