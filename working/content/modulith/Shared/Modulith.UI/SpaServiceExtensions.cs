using MudBlazor.Services;

namespace Modulith.UI;

public static class SpaServiceExtensions
{
  public static IServiceCollection RegisterNewModuleSpaServices(this IServiceCollection services)
  {
    services.AddMudServices();

    return services;
  }
}
