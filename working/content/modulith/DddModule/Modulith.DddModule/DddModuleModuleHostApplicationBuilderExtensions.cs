using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modulith.DddModule.Api;
using Modulith.DddModule.Infrastructure;
using Modulith.SharedKernel;

namespace Modulith.DddModule;

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
