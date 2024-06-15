using Modulith.UI;
using Modulith.UI.Pages;
using Modulith.Web.Components;

namespace Modulith.Web;

public static class ConventionBuilderExtensions
{
  public static WebApplication AddBlazorModulesAdditionalAssemblies(this WebApplication app)
  {
    var discoveryService = app.Services.GetRequiredService<IBlazorAssemblyDiscoveryService>();
    
    var componentBuilder = app.MapRazorComponents<App>()
      .AddInteractiveServerRenderMode()
      .AddInteractiveWebAssemblyRenderMode()
      .AddAdditionalAssemblies(typeof(Counter).Assembly);

    discoveryService.GetAssemblies()
      .ToList()
      .ForEach( a => componentBuilder.AddAdditionalAssemblies(a));

    return app;
  }
}
