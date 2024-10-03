using _Modulith_.SharedKernel;
using _Modulith_._BlazorModule_.HttpModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace _Modulith_._BlazorModule_.UI;

public class _BlazorModule_UiModuleServiceRegistrar : IRegisterModuleServices
{
  public static IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IWeatherForecastService, ClientWeatherForecastService>();

    return services;
  }
}
