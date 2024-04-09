using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Modulith.Module1;

public static class Module1ModuleHostApplicationBuilderExtensions
{
  public static void AddModule1Services(this IHostApplicationBuilder builder)
  {
    var logger = GetLogger(builder);
    
    logger.LogInformation("⚙️ Module1 module services registered");
  }
  
  private static ILogger<WebApplicationBuilder> GetLogger(IHostApplicationBuilder builder) 
    => builder.Services
      .BuildServiceProvider()
      .GetRequiredService<ILogger<WebApplicationBuilder>>();
}
