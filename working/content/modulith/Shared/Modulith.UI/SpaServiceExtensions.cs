using Modulith.NewModule.HttpModels;
using Modulith.NewModule.UI;
using MudBlazor.Services;

namespace Modulith.UI;

public static class SpaServiceExtensions
{
  public static IServiceCollection RegisterNewModuleSpaServices(this IServiceCollection services)
  {
    services.AddMudServices();
    services.AddBlazorAssemblyDiscovery();
    
    services.AddScoped<IWeatherForecastService, ClientWeatherForecastService>();

    return services;
  }
}
