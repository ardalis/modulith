using eShop.Payments.HttpModels;
using Microsoft.Extensions.DependencyInjection;
using eShop.SharedKernel;

namespace eShop.Payments;

public class PaymentsModuleServiceRegistrar : IRegisterModuleServices
{
  public static IServiceCollection ConfigureServices(IServiceCollection services)
  {
    services.AddMediatR(
      c => c.RegisterServicesFromAssemblies(typeof(AssemblyInfo).Assembly));

    services.AddScoped<IWeatherForecastService, ServerWeatherForecastService>();

    return services;
  }
}
