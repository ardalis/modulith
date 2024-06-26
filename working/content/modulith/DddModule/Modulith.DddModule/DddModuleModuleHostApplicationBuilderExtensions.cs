using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modulith.NewModule.Api;
using Modulith.SharedKernel;

namespace Modulith.NewModule;

public class NewModuleServiceRegistrar : IRegisterModuleServices
{
  public static IHostApplicationBuilder ConfigureServices(IHostApplicationBuilder builder)
  {
    var logger = GetLogger(builder);
    builder.Services.AddMediatR(
      c => c.RegisterServicesFromAssemblies(typeof(AssemblyInfo).Assembly));

    builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
    
    logger.LogInformation("⚙️ NewModule module services registered");

    return builder;
  }
  
  private static ILogger<WebApplicationBuilder> GetLogger(IHostApplicationBuilder builder) 
    => builder.Services
      .BuildServiceProvider()
      .GetRequiredService<ILogger<WebApplicationBuilder>>();
}
