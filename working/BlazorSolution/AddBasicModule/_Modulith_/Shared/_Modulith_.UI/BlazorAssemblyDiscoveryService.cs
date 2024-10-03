using System.Reflection;
using _Modulith_.NewModule.UI;
using _Modulith_.UI;

namespace _Modulith_.UI;

public class BlazorAssemblyDiscoveryService : IBlazorAssemblyDiscoveryService
{
  public IEnumerable<Assembly> GetAssemblies() => [typeof(NewModuleComponent).Assembly];
}
