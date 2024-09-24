using Microsoft.Extensions.DependencyInjection;

namespace _Modulith_.SharedKernel;

public interface IRegisterModuleServices
{
  static abstract IServiceCollection ConfigureServices(IServiceCollection services);
}
