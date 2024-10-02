using _Modulith_.SharedKernel;
using _Modulith_.NewModule.HttpModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace _Modulith_.NewModule.UI;

public class NewModuleUiModuleServiceRegistrar : IRegisterModuleServices
{
  public static IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IWeatherForecastService, ClientWeatherForecastService>();

    return services;
  }
}