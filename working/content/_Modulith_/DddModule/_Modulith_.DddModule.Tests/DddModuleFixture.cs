using FastEndpoints.Testing;
using _Modulith_.Web;

namespace _Modulith_.DddModule.Tests;

public class DddModuleFixture : AppFixture<Program>
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
