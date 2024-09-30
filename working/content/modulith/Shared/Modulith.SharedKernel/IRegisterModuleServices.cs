using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Modulith.SharedKernel;

public interface IRegisterModuleServices
{
  static abstract IServiceCollection ConfigureServices(IServiceCollection services,
    IConfiguration configuration);
}
