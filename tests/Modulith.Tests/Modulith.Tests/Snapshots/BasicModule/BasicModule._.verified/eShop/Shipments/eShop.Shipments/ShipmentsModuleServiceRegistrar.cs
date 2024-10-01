using eShop.Shipments.HttpModels;
using Microsoft.Extensions.DependencyInjection;
using eShop.SharedKernel;
using Microsoft.Extensions.Configuration;

namespace eShop.Shipments;

public class ShipmentsModuleServiceRegistrar : IRegisterModuleServices
{
  public static IServiceCollection ConfigureServices(IServiceCollection services,
    IConfiguration configuration)
  {
    services.AddMediatR(
      c => c.RegisterServicesFromAssemblies(typeof(AssemblyInfo).Assembly));

    services.AddScoped<IWeatherForecastService, ServerWeatherForecastService>();

    return services;
  }
}
