using System.Reflection;
using Modulith.SharedKernel;

namespace Modulith.Web;

public static class WebApplicationBuilderExtensions
{
  private static string SolutionName => "Modulith";

  public static void DiscoverAndRegisterModules(this WebApplicationBuilder webApplicationBuilder)
  {
    var logger = CreateLogger(webApplicationBuilder);

    var discoveredModuleAssemblies = DiscoverModuleAssemblies(logger);

    var moduleAssembliesLoaded = LoadAssembliesToApp(discoveredModuleAssemblies, logger);

    moduleAssembliesLoaded.ForEach(module => RegisterModuleServices(webApplicationBuilder, module, logger));
  }

  private static ILogger CreateLogger(WebApplicationBuilder webApplicationBuilder)
    => LoggerFactory.Create(config =>
    {
      config.AddConsole();
      config.AddConfiguration(webApplicationBuilder.Configuration.GetSection("Logging"));
    }).CreateLogger(nameof(WebApplicationBuilderExtensions));

  private static List<AssemblyName> DiscoverModuleAssemblies(ILogger logger)
  {
    var solutionAssemblies = GetAllSolutionAssemblies();
    var paths              = GetAssembliesPaths(solutionAssemblies);

    var discoveredAssemblies = GetDiscoveredAssemblies(logger, paths);

    logger.LogDebug("🧩 Found the following assembly modules");
    discoveredAssemblies.ForEach(d => logger.LogDebug("🧩 {name}", d.Name));

    return discoveredAssemblies;
  }

  private static IEnumerable<string> GetAppAssemblies()
    => AppDomain.CurrentDomain.GetAssemblies().ToList().Select(a => a.Location).ToArray();

  private static void RegisterModuleServices(WebApplicationBuilder webApplicationBuilder, Assembly assembly, ILogger logger)
  {
    if (!TryGetServiceRegistrationMethod(logger, assembly, out var method))
    {
      logger.LogError("🛑 An error occurred registering services for assembly: '{assembly}'. Skipping registration", assembly.GetName());
      return;
    }

    InvokeServiceRegistrationMethod(logger, webApplicationBuilder, method!);
  }

  private static void InvokeServiceRegistrationMethod(ILogger logger, WebApplicationBuilder webApplicationBuilder, MethodInfo method)
  {
    try
    {
      var initialServiceCount = GetServicesCount(webApplicationBuilder);
      method.Invoke(null, [webApplicationBuilder]);
      var finalServiceCount = GetServicesCount(webApplicationBuilder);

      logger.LogInformation("✅ Registered {serviceCount} services for module: {module}",
        finalServiceCount - initialServiceCount,
        $"{method.DeclaringType?.Assembly.GetName().Name}" ?? method.Name);
    }
    catch (Exception)
    {
      logger.LogError($"An exception occured when invoking {method.Name}. Try calling the method directly from Program.cs");
      throw;
    }
  }

  private static int GetServicesCount(WebApplicationBuilder webApplicationBuilder)
    => webApplicationBuilder.Services.GroupBy(s => s.ServiceType).Count();

  private static bool TryGetServiceRegistrationMethod(ILogger logger, Assembly assembly, out MethodInfo? method)
  {
    method = default;

    if (!TryGetModuleName(logger, assembly, out var moduleName)) return false;

    if (!TryGetServiceRegistrationClass(logger, assembly, out var serviceRegistrationClass)) return false;

    var serviceRegistrationMethod = $"Add{moduleName}Services";
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

  private static bool HasCorrectSignature(MethodInfo m)
  {

    return m.GetParameters().SingleOrDefault()?.ParameterType == typeof(WebApplicationBuilder);
  }

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

  private static bool TryGetModuleName(ILogger logger, Assembly assembly, out string moduleName)
  {
    moduleName = string.Empty;
    if (!TryGetModuleAssemblyName(logger, assembly, out var moduleAssembly)) return false;

    moduleName = moduleAssembly.Split($"{SolutionName}.")[1];
    if (string.IsNullOrEmpty(moduleAssembly))
    {
      logger.LogError($"Could not find module {moduleName}");
      return false;
    }

    logger.LogDebug($"🐞 Registering services for module '{moduleName}' in '{assembly.GetName().Name}'",
      moduleName, moduleAssembly);

    return true;
  }

  private static bool TryGetModuleAssemblyName(ILogger logger, Assembly assembly, out string moduleAssembly)
  {
    moduleAssembly = assembly.GetName().Name ?? string.Empty;
    if (string.IsNullOrEmpty(moduleAssembly))
    {
      logger.LogError($"Could not find assembly {moduleAssembly}");
      return false;
    }

    return true;
  }

  private static List<Assembly> LoadAssembliesToApp(List<AssemblyName> assemblyModuleNames, ILogger logger)
  {
    var addedAssemblies = new List<Assembly>();
    assemblyModuleNames.ForEach(assemblyName =>
    {
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

  private static List<string> GetAssembliesPaths(IEnumerable<string> solutionAssemblies)
  {
    var loadedPaths = GetAppAssemblies();
    return solutionAssemblies.Where(r => IsModuleAssembly(loadedPaths, r)).ToList();
  }

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

  private static bool IsModuleAssembly(IEnumerable<string> loadedPaths, string r)
    => !(IsWebOrTestAssembly(r) || IsSharedKernel(r));

  private static bool IsSharedKernel(string r)
    => r.Contains("SharedKernel");

  private static bool IsWebOrTestAssembly(string r)
    => r.Contains(".Web.") || r.Contains(".Tests.");
}
