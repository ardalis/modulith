using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Modulith.SharedKernel;

public interface IRegisterModuleServices
{
  static abstract IHostApplicationBuilder ConfigureServices(IHostApplicationBuilder builder);

  private static ILogger<WebApplicationBuilder> GetLogger(IHostApplicationBuilder builder)
    => builder.Services
      .BuildServiceProvider()
      .GetRequiredService<ILogger<WebApplicationBuilder>>();
}
