using Microsoft.Extensions.DependencyInjection;

namespace eShop.SharedKernel;

public interface IRegisterModuleServices
{
  static abstract IServiceCollection ConfigureServices(IServiceCollection services);
}
