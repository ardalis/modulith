using Microsoft.Extensions.Options;
using Microsoft.TemplateEngine.Authoring.TemplateVerifier;
using Xunit.Abstractions;

namespace Modulith.Tests;

public class SolutionTests(ITestOutputHelper output) : TestBase(output)
{
  [Fact]
  public async Task SolutionWithUi()
  {
    await Engine.Execute(WithVerificationOptions([
      "--name", "eShop",
      "--module-name", "Payments",
      "--with-ui"
    ]));
  }

  [Fact]
  public async Task BasicModule()
  {
    var solutionOptions = WithoutVerificationOptions([
      "--name", "eShop",
      "--module-name", "Payments",
      "--with-ui",
      "-o", "eShop"
    ]);
    await Engine.Execute(solutionOptions);

    await Engine.Execute(WithVerificationOptions([
      "--template", "basic",
      "--module-name", "Shipments",
      "--with-ui",
      "-o", "eShop/eShop.Web"
    ], solutionOptions.OutputDirectory));
  }
}
