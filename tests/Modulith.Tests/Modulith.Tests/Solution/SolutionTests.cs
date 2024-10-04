using Modulith.Tests.Extensions;
using Xunit.Abstractions;

namespace Modulith.Tests.Solution;

public class SolutionTests(ITestOutputHelper output) : TestBase(output)
{
  [Fact]
  public async Task Solution()
  {
    await Engine.Verify(options =>
      options.WithArgs([
          "--name", "eShop",
          "--module-name", "Payments",
          "-o", "eShop"
        ])
        .DisableDiffTool(false)
        .DeletingVerifyCompilationFiles());
  }

  [Fact]
  public async Task WithBasicModule()
  {
    var outputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    var solutionOptions = await Engine.TryVerify(o =>
      o.WithArgs([
          "--name", "eShop",
          "--module-name", "Payments",
          "-o", "eShop",
        ])
        .DeletingVerifyCompilationFiles()
        .DeletingReceivingDirectory()
        .WithOutputDirectory(outputDirectory)
    );

    await Engine.Verify(
      o => o.WithArgs([
          "--template", "basic",
          "--module-name", "Shipments",
          "-o", "eShop/eShop.Web",
        ])
        .DisableDiffTool(false)
        .WithOutputDirectory(solutionOptions.OutputDirectory!));
  }
}
