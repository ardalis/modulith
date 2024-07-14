using System.Reflection;
using eShop.SharedKernel;

namespace Modulith.UI;

public static class ServiceCollectionExtensions
{
  public static void AddBlazorAssemblyDiscovery(this IServiceCollection services) => services.AddSingleton<IBlazorAssemblyDiscoveryService, BlazorAssemblyDiscoveryService>();

  public static void RegisterClientSideServices(this IServiceCollection services)
  {
    var logger           = CreateLogger();
    var discoveryService = services.BuildServiceProvider().GetRequiredService<IBlazorAssemblyDiscoveryService>();

    var assemblies = discoveryService.GetAssemblies().ToList();

    assemblies.ForEach(module => RegisterModuleServices(services, module, logger));

  }

  private static ILogger CreateLogger()
    => LoggerFactory.Create(_ => {}).CreateLogger(nameof(ServiceCollectionExtensions));

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


  private static MethodInfo? GetRegistrationMethod(IReflect? serviceRegistrationClass)
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
}
