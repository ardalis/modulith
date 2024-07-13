namespace Modulith.UI;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddBlazorAssemblyDiscovery(this IServiceCollection services)
  {
    services.AddSingleton<IBlazorAssemblyDiscoveryService, BlazorAssemblyDiscoveryService>();

    return services;
  }
}
