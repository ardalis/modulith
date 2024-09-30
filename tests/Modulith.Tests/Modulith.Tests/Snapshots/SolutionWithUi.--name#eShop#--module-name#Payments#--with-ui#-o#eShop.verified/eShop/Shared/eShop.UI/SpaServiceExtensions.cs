using MudBlazor.Services;

namespace eShop.UI;

public static class SpaServiceExtensions
{
  public static IServiceCollection RegisterPaymentsSpaServices(this IServiceCollection services)
  {
    services.AddMudServices();
    services.AddBlazorAssemblyDiscovery();

    return services;
  }
}
