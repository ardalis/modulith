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
      ]);
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
      ]);
    await Engine.Execute(options.Build());
  }
  
  [Fact]
  public async Task BasicModule()
  {
    var solutionDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    var solutionOptions = GetVerificationOptions($"{nameof(BasicModule)}._.received")
      .WithArgs([
        "--name", "eShop",
        "--module-name", "Payments",
        "--with-ui",
        "-o", "eShop"
      ])
      .WithOutputDirectory(solutionDir)
      .InstantiateWithoutVerification()
      .Build();

    await Engine.Execute(solutionOptions);

    var moduleOptions = GetVerificationOptions()
      .WithArgs([
        "--template", "basic",
        "--module-name", "Shipments",
        "--with-ui",
        "-o", "eShop/eShop.Web"
      ])
      .WithOutputDirectory(solutionDir)
      .Build();

    await Engine.Execute(moduleOptions);
  }
}
