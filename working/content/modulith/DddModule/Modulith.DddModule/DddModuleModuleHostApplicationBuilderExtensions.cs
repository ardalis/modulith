using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modulith.DddModule.Api;
using Modulith.DddModule.Infrastructure;
using Modulith.SharedKernel;

namespace Modulith.DddModule;

public class DddModuleServiceRegistrar : IRegisterModuleServices
{
  public static IHostApplicationBuilder ConfigureServices(IHostApplicationBuilder builder)
  {
    var logger = GetLogger(builder);
    builder.Services.AddMediatR(
      c => c.RegisterServicesFromAssemblies(typeof(AssemblyInfo).Assembly));

    builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
    builder.Services.AddScoped<ITemperatureService, FakeTemperatureService>();
    
    logger.LogInformation("⚙️ DddModule module services registered");

    return builder;
  }
  
  private static ILogger<WebApplicationBuilder> GetLogger(IHostApplicationBuilder builder) 
    => builder.Services
      .BuildServiceProvider()
      .GetRequiredService<ILogger<WebApplicationBuilder>>();
}
