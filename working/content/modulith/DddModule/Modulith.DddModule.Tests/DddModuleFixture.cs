using FastEndpoints.Testing;
using Modulith.Web;

namespace Modulith.NewModule.Tests;

public class NewModuleFixture : AppFixture<Program>
{
  protected override async Task SetupAsync()
  {
    Client = CreateClient();

    await base.SetupAsync();
  }

  protected override async Task TearDownAsync()
  {
    Client.Dispose();
    await base.TearDownAsync();
  }
}
