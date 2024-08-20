using System.Reflection;
using Modulith.SharedKernel;

namespace Modulith.Web;

public static class ModuleRegistrationExtensions
{
  private static string SolutionName => "Modulith";

  public static void DiscoverAndRegisterModules(this IServiceCollection services)
  {
    var logger = CreateLogger();

    var discoveredModuleAssemblies = DiscoverModuleAssemblies(logger);

    var moduleAssembliesLoaded = LoadAssembliesToApp(discoveredModuleAssemblies, logger);

    moduleAssembliesLoaded.ForEach(module => RegisterModuleServices(services, module, logger));
  }

  private static ILogger CreateLogger()
    => LoggerFactory.Create(config => {
        config.AddConsole();
      })
      .CreateLogger(nameof(ModuleRegistrationExtensions));

  private static List<AssemblyName> DiscoverModuleAssemblies(ILogger logger)
  {
    var solutionAssemblies = GetAllSolutionAssemblies();
    var paths              = GetAssembliesPaths(solutionAssemblies);

    var discoveredAssemblies = GetDiscoveredAssemblies(logger, paths);

    logger.LogDebug("🧩 Found the following assembly modules");
    discoveredAssemblies.ForEach(d => logger.LogDebug("🧩 {name}", d.Name));

    return discoveredAssemblies;
  }

  private static void RegisterModuleServices(IServiceCollection services, Assembly assembly, ILogger logger)
  {
    if (!TryGetServiceRegistrationMethod(logger, assembly, out var method))
    {
      logger.LogError("🛑 An error occurred registering services for assembly: '{assembly}'. Skipping registration", assembly.GetName());
      return;
    }

    InvokeServiceRegistrationMethod(logger, services, method!);
  }

  private static void InvokeServiceRegistrationMethod(ILogger logger, IServiceCollection services, MethodBase method)
  {
    try
    {
      var initialServiceCount = GetServicesCount(services);
      method.Invoke(null, [services]);
      var finalServiceCount = GetServicesCount(services);

      logger.LogInformation("✅ Registered {serviceCount} services for module: {module}",
        finalServiceCount - initialServiceCount,
        $"{method.DeclaringType?.Assembly.GetName().Name}");
    }
    catch (Exception)
    {
      logger.LogError($"An exception occured when invoking {method.Name}. Try calling the method directly from Program.cs");
      throw;
    }
  }

  private static int GetServicesCount(IServiceCollection services)
    => services.GroupBy(s => s.ServiceType).Count();

  private static bool TryGetServiceRegistrationMethod(ILogger logger, Assembly assembly, out MethodInfo? method)
  {
    method = default;

    if (!TryGetServiceRegistrationClass(logger, assembly, out var serviceRegistrationClass))
    {
      return false;
    }

    method = GetRegistrationMethod(serviceRegistrationClass);
    if (method == default)
    {
      logger.LogError($"Could not find extensions method '{nameof(IRegisterModuleServices.ConfigureServices)}'");
      return false;
    }

    return true;
  }

  private static MethodInfo? GetRegistrationMethod(Type? serviceRegistrationClass)
    => serviceRegistrationClass!.GetMethod(nameof(IRegisterModuleServices.ConfigureServices), BindingFlags.Static | BindingFlags.Public);

  private static bool TryGetServiceRegistrationClass(ILogger logger, Assembly assembly, out Type? serviceRegistrationClass)
  {
    serviceRegistrationClass = GetRegisterServicesClass(assembly);

    if (serviceRegistrationClass == default)
    {
      logger.LogError("Could not find a public class that implements the interface: {IRegisterServices}", nameof(IRegisterModuleServices));
      return false;
    }

    logger.LogDebug("Found '{serviceRegistrationClass}' using it to register modules from: '{assembly}'",
      serviceRegistrationClass.Name,
      assembly.GetName().Name);
    return true;
  }

  private static Type? GetRegisterServicesClass(Assembly assembly)
    => assembly.GetTypes().FirstOrDefault(t => t.IsClass && t.IsAssignableTo(typeof(IRegisterModuleServices)));

  private static List<Assembly> LoadAssembliesToApp(List<AssemblyName> assemblyModuleNames, ILogger logger)
  {
    var addedAssemblies = new List<Assembly>();
    assemblyModuleNames.ForEach(assemblyName => {
      logger.LogDebug("🐞 Loading module from assembly: {assembly}", $"{assemblyName.Name} {assemblyName.Version}");
      try
      {
        var assembly = AppDomain.CurrentDomain.Load(assemblyName);
        addedAssemblies.Add(assembly);
      }
      catch (Exception)
      {
        logger.LogError(
          "🛑 Could not load assembly to app: {assembly}. This might be an old assembly no longer in use. Cleaning bin/ and obj/ might solve this issue. Otherwise, try to call the the module registration method directly from program.cs",
          assemblyName.Name);
      }

    });
    return addedAssemblies;
  }

  private static List<string> GetAssembliesPaths(IEnumerable<string> solutionAssemblies) => solutionAssemblies.Where(IsModuleAssembly).ToList();

  private static string[] GetAllSolutionAssemblies()
    => Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"{SolutionName}.*.dll");

  private static List<AssemblyName> GetDiscoveredAssemblies(ILogger logger, List<string> assemblyModulePaths)
    => assemblyModulePaths.Select(p => GetAssemblyName(logger, p)).Where(p => p is not null).ToList()!;

  private static AssemblyName? GetAssemblyName(ILogger logger, string path)
  {
    try
    {
      return AssemblyName.GetAssemblyName(path);
    }
    catch (Exception)
    {
      logger.LogError(
        "🛑 Could not load assembly metadata for: {path}. This might be an old assembly. Cleaning bin/ and obj/ might solve this issue. Otherwise, try to call the the module registration method directly from program.cs. And make sure there is a reference to the module project.",
        path);
    }

    return null;
  }

  private static bool IsModuleAssembly(string r)
    => !(IsWebOrTestAssembly(r) || IsSharedKernel(r) || IsUi(r));
  private static bool IsUi(string s) => s.Contains(".UI.") || s.Contains(".HttpModels.");

  private static bool IsSharedKernel(string r)
    => r.Contains("SharedKernel");

  private static bool IsWebOrTestAssembly(string r)
    => r.Contains(".Web.") || r.Contains(".Tests.");
}
