using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using _Modulith_.DddModule.Api;
using _Modulith_.DddModule.Infrastructure;
using _Modulith_.SharedKernel;

namespace _Modulith_.DddModule;

public class DddModuleServiceRegistrar : IRegisterModuleServices
{
  public static IServiceCollection ConfigureServices(IServiceCollection services)
  {
    var logger = GetLogger(services);
    services.AddMediatR(
      c => c.RegisterServicesFromAssemblies(typeof(AssemblyInfo).Assembly));

    services.AddScoped<IWeatherForecastService, WeatherForecastService>();
    services.AddScoped<ITemperatureService, FakeTemperatureService>();

    logger.LogInformation("⚙️ DddModule module services registered");

    return services;
  }

  private static ILogger<IServiceCollection> GetLogger(IServiceCollection services)
    => services
      .BuildServiceProvider()
      .GetRequiredService<ILogger<IServiceCollection>>();
}
