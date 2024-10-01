using Xunit.Abstractions;

namespace Modulith.Tests;

public class SolutionTests(ITestOutputHelper output) : TestBase(output)
{
  [Fact]
  public async Task SolutionWithUi()
  {
    await Engine.Execute(options => options
      .WithArgs([
        "--name", "eShop",
        "--module-name", "Payments",
        "--with-ui",
        "-o", "eShop"
      ])
      .DisableDiffTool(false)
      .DeletingReceivingDirectory());
  }

  [Fact]
  public async Task Solution()
  {
    await Engine.Execute(options => 
      options.WithArgs([
        "--name", "eShop",
        "--module-name", "Payments",
        "-o", "eShop"
      ])
      .DeletingVerifyCompilationFiles()
      .DisableDiffTool(false)
      .WithOutputDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()))
      .DeletingReceivingDirectory());
  }

  [Fact]
  public async Task BasicModule()
  {
    var outputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      var solutionOptions = await Engine.TryExecute(o => 
        o.WithArgs([
          "--name", "eShop",
          "--module-name", "Payments",
          "--with-ui",
          "-o", "eShop"
        ])
        .DeletingVerifyCompilationFiles()
        .DeletingReceivingDirectory()
        .KeepInstantiationInSnapshot()
        // .WithOutputDirectory(outputDirectory)
        );

    await Engine.Execute(o => o.WithArgs([
        "--template", "basic",
        "--module-name", "Shipments",
        "--with-ui",
        "-o", "eShop/eShop.Web"
      ])
      .DisableDiffTool(false)
      .WithOutputDirectory(solutionOptions.OutputDirectory!));
  }
}