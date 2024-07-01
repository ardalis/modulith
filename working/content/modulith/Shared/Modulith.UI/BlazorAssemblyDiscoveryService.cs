using System.Reflection;
using Modulith.NewModule.UI;
using Modulith.UI;

namespace Modulith.UI;

public class BlazorAssemblyDiscoveryService : IBlazorAssemblyDiscoveryService
{
  public IEnumerable<Assembly> GetAssemblies()
  {
      return [typeof(NewModuleComponent).Assembly];
  }
}

