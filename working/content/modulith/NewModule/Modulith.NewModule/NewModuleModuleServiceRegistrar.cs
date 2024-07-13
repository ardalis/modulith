using Modulith.NewModule.HttpModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modulith.SharedKernel;

namespace Modulith.NewModule;

public class NewModuleModuleServiceRegistrar : IRegisterModuleServices
{
  public static IHostApplicationBuilder ConfigureServices(IHostApplicationBuilder builder)
  {
    builder.Services.AddMediatR(
      c => c.RegisterServicesFromAssemblies(typeof(AssemblyInfo).Assembly));
      
    builder.Services.AddScoped<IWeatherForecastService, ServerWeatherForecastService>();

    return builder;
  }
}