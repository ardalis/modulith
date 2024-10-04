using Modulith.Tests.Extensions;
using Xunit.Abstractions;

namespace Modulith.Tests.Solution;

public class V1(ITestOutputHelper output) : TestBase(output)
{
  [Fact]
  public async Task Solution()
  {
    // dotnet new modulith -n eShop --with-module Payments
    await Engine.Verify(options =>
      options.WithArgs([
          "-n", "eShop",
          "--with-module", "Payments",
          "-o", "eShop"
        ])
        .DisableDiffTool()
        .DeletingVerifyCompilationFiles());
  }

  [Fact]
  public async Task WithBasicModule()
  {
    // var outputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    var solutionOptions = await Engine.TryVerify(o =>
      o.WithArgs([
          "-n", "eShop",
          "--with-module", "Payments",
          "-o", "eShop"
        ])
        .KeepInstantiationInSnapshot()
        .DeletingVerifyCompilationFiles()
        .DeletingReceivingDirectory()
        // .WithOutputDirectory(outputDirectory)
    );

    // dotnet new modulith --add basic-module --with-name Shipments --to eShop
    await Engine.Verify(
      o => o.WithArgs([
          "--add", "basic-module",
          "--with-name", "Shipments",
          "--to", "eShop",
          "-o", "eShop/eShop.Web",
        ])
        .DisableDiffTool(false)
        .WithOutputDirectory(solutionOptions.OutputDirectory!));
  }
}
