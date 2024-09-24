using eShop.Shipments.HttpModels;
using Microsoft.Extensions.DependencyInjection;
using eShop.SharedKernel;

namespace eShop.Shipments;

public class ShipmentsModuleServiceRegistrar : IRegisterModuleServices
{
  public static IServiceCollection ConfigureServices(IServiceCollection services)
  {
    services.AddMediatR(
      c => c.RegisterServicesFromAssemblies(typeof(AssemblyInfo).Assembly));

    services.AddScoped<IWeatherForecastService, ServerWeatherForecastService>();

    return services;
  }
}
