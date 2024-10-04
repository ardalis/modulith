using Microsoft.Extensions.DependencyInjection;
using _Modulith_.SharedKernel;
using Microsoft.Extensions.Configuration;

namespace _Modulith_._NewModule_;

public class _NewModule_ModuleServiceRegistrar : IRegisterModuleServices
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
