using System.Reflection;

namespace eShop.UI;

public interface IBlazorAssemblyDiscoveryService
{
  IEnumerable<Assembly> GetAssemblies();
}
