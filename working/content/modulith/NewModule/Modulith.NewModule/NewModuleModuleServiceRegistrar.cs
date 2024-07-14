using Modulith.NewModule.HttpModels;
using Microsoft.Extensions.DependencyInjection;
using Modulith.SharedKernel;

namespace Modulith.NewModule;

public class NewModuleModuleServiceRegistrar : IRegisterModuleServices
{
  public static IServiceCollection ConfigureServices(IServiceCollection services)
  {
    services.AddMediatR(
      c => c.RegisterServicesFromAssemblies(typeof(AssemblyInfo).Assembly));

    services.AddScoped<IWeatherForecastService, ServerWeatherForecastService>();

    return services;
  }
}
