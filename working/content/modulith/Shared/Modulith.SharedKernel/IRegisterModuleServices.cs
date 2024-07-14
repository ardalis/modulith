using Microsoft.Extensions.DependencyInjection;

namespace Modulith.SharedKernel;

public interface IRegisterModuleServices
{
  static abstract IServiceCollection ConfigureServices(IServiceCollection services);
}
