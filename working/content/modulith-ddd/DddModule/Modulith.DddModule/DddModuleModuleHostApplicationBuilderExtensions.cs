using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modulith.DddModule.Api;

namespace Modulith.DddModule;

public static class DddModuleModuleHostApplicationBuilderExtensions
{
  public static void AddDddModuleServices(this IHostApplicationBuilder builder)
  {
    var logger = GetLogger(builder);
    builder.Services.AddMediatR(
      c => c.RegisterServicesFromAssemblies(typeof(AssemblyInfo).Assembly));

    builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
    
    logger.LogInformation("⚙️ DddModule module services registered");
  }
  
  private static ILogger<WebApplicationBuilder> GetLogger(IHostApplicationBuilder builder) 
    => builder.Services
      .BuildServiceProvider()
      .GetRequiredService<ILogger<WebApplicationBuilder>>();
}
