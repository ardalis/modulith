using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace _Modulith_.SharedKernel;

public interface IRegisterModuleServices
{
  static abstract IServiceCollection ConfigureServices(IServiceCollection services,
    IConfiguration configuration);
}
