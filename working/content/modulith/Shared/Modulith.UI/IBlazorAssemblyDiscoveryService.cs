using System.Reflection;

namespace Modulith.UI;

public interface IBlazorAssemblyDiscoveryService
{
  IEnumerable<Assembly> GetAssemblies();
}
