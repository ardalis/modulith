using eShop.UI;
using eShop.UI.Pages;
using eShop.Web.Components;

namespace eShop.Web;

public static class ConventionBuilderExtensions
{
  public static void AddBlazorModulesAdditionalAssemblies(this WebApplication app)
  {
    var discoveryService = app.Services.GetRequiredService<IBlazorAssemblyDiscoveryService>();

    var componentBuilder = app.MapRazorComponents<App>()
      .AddInteractiveServerRenderMode()
      .AddInteractiveWebAssemblyRenderMode()
      .AddAdditionalAssemblies(typeof(Counter).Assembly);

    discoveryService.GetAssemblies()
      .ToList()
      .ForEach(a => componentBuilder.AddAdditionalAssemblies(a));

  }
}
