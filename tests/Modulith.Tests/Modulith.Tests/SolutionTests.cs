using Microsoft.Extensions.Options;
using Microsoft.TemplateEngine.Authoring.TemplateVerifier;
using Xunit.Abstractions;
using static Modulith.Tests.TemplateVerifierOptionsExtensions;

namespace Modulith.Tests;

public class SolutionTests(ITestOutputHelper output) : TestBase(output)
{
  [Fact]
  public async Task SolutionWithUi()
  {
    var options = GetVerificationOptions()
      .WithArgs([
        "--name", "eShop",
        "--module-name", "Payments",
        "--with-ui",
        "-o", "eShop"
      ])
      .DeletingOutputDirectory();
    await Engine.Execute(options.Build());
  }
  
  [Fact]
  public async Task Solution()
  {
    var options = GetVerificationOptions()
      .WithArgs([
        "--name", "eShop",
        "--module-name", "Payments",
        "-o", "eShop"
      ])
      .DeletingOutputDirectory();
    await Engine.Execute(options.Build());
  }
  
  [Fact]
  public async Task BasicModule()
  {
    var solutionOptions = GetVerificationOptions()
      .WithArgs([
        "--name", "eShop",
        "--module-name", "Payments",
        "--with-ui",
        "-o", "eShop"
      ])
      .InstantiateWithoutVerification()
      .DeletingOutputDirectory()
      .Build();

    await Engine.Execute(solutionOptions);

    var moduleOptions = GetVerificationOptions()
      .WithArgs([
        "--template", "basic",
        "--module-name", "Shipments",
        "--with-ui",
        "-o", "eShop/eShop.Web"
      ])
      .WithOutputDirectory(solutionOptions.OutputDirectory!)
      .EnsureEmptyOutputDirectory(false)
      .Build();

    await Engine.Execute(moduleOptions);
  }
}
