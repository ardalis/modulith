using eShop.SharedKernel;
using eShop.Payments.HttpModels;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Payments.UI;

public class PaymentsUiModuleServiceRegistrar : IRegisterModuleServices
{
  public static IServiceCollection ConfigureServices(IServiceCollection services)
  {
    services.AddScoped<IWeatherForecastService, ClientWeatherForecastService>();

    return services;
  }
}
