using _Modulith_.SharedKernel;
using _Modulith_.NewModule.HttpModels;
using Microsoft.Extensions.DependencyInjection;

namespace _Modulith_.NewModule.UI;

public class NewModuleUiModuleServiceRegistrar : IRegisterModuleServices
{
  public static IServiceCollection ConfigureServices(IServiceCollection services)
  {
    services.AddScoped<IWeatherForecastService, ClientWeatherForecastService>();

    return services;
  }
}
