using Modulith.SharedKernel;
using Modulith.NewModule.HttpModels;
using Microsoft.Extensions.DependencyInjection;

namespace Modulith.NewModule.UI;

public class NewModuleUiModuleServiceRegistrar : IRegisterModuleServices
{
  public static IServiceCollection ConfigureServices(IServiceCollection services)
  {
    services.AddScoped<IWeatherForecastService, ClientWeatherForecastService>();

    return services;
  }
}
