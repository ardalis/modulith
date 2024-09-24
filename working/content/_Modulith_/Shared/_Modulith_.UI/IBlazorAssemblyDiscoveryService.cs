using System.Reflection;

namespace _Modulith_.UI;

public interface IBlazorAssemblyDiscoveryService
{
  IEnumerable<Assembly> GetAssemblies();
}
