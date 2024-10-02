using MudBlazor.Services;

namespace _Modulith_.UI;

public static class SpaServiceExtensions
{
  public static IServiceCollection RegisterNewModuleSpaServices(this IServiceCollection services)
  {
    services.AddMudServices();
    services.AddBlazorAssemblyDiscovery();

    return services;
  }
}
