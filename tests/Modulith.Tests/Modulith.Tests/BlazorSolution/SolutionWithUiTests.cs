using Modulith.Tests.Extensions;
using Xunit.Abstractions;

namespace Modulith.Tests.SolutionWithUi;

public class SolutionWithUiTests(ITestOutputHelper output) : TestBase(output)
{
  [Fact]
  public async Task BlazorSolution()
  {
    await Engine.Verify(options => options
      .WithArgs([
        "--template", "blazor-solution",
        "--name", "eShop",
        "--module-name", "Payments",
        "-o", "eShop"
      ])
      .DisableDiffTool(false)
      .DeletingReceivingDirectory());
  }

  [Fact]
  public async Task WithBasicModule()
  {
    var outputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    await Engine.TryVerify(o =>
        o.WithArgs([
            "--template", "blazor-solution",
            "--name", "eShop",
            "--module-name", "Payments",
            "-o", "eShop"
          ])
          .DeletingVerifyCompilationFiles()
          .DeletingReceivingDirectory()
          .WithOutputDirectory(outputDirectory) // This is necessary for both instantiations to point to the same location
    );
    
    await Engine.Verify(o => o.WithArgs([
        "--template", "blazor-module",
        "--module-name", "Shipments",
        "-o", "eShop/eShop.Web"
      ])
      // .DisableDiffTool()
      .WithOutputDirectory(outputDirectory!));
  }
}
