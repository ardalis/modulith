using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modulith.NewModule.Api;

namespace Modulith.NewModule;

public static class NewModuleModuleHostApplicationBuilderExtensions
{
  public static void AddNewModuleServices(this IHostApplicationBuilder builder)
  {
    var logger = GetLogger(builder);
    builder.Services.AddMediatR(
      c => c.RegisterServicesFromAssemblies(typeof(AssemblyInfo).Assembly));

    builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
    
    logger.LogInformation("⚙️ NewModule module services registered");
  }
  
  private static ILogger<WebApplicationBuilder> GetLogger(IHostApplicationBuilder builder) 
    => builder.Services
      .BuildServiceProvider()
      .GetRequiredService<ILogger<WebApplicationBuilder>>();
}
