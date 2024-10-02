using _Modulith_.UI;
using _Modulith_.UI.Pages;
using _Modulith_.Web.Components;

namespace _Modulith_.Web;

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
