using FastEndpoints.Testing;
using Modulith.Web;

namespace Modulith.DddModule.Tests;

public class DddModuleFixture : AppFixture<Program>
{
  protected override async ValueTask SetupAsync()
  {
    Client = CreateClient();

    await base.SetupAsync();
  }

  protected override async ValueTask TearDownAsync()
  {
    Client.Dispose();
    await base.TearDownAsync();
  }
}
