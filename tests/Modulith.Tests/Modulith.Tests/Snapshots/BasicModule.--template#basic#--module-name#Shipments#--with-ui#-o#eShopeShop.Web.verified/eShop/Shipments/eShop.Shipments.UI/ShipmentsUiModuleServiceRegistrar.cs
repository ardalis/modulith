using eShop.SharedKernel;
using eShop.Shipments.HttpModels;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Shipments.UI;

public class ShipmentsUiModuleServiceRegistrar : IRegisterModuleServices
{
  public static IServiceCollection ConfigureServices(IServiceCollection services)
  {
    services.AddScoped<IWeatherForecastService, ClientWeatherForecastService>();

    return services;
  }
}
