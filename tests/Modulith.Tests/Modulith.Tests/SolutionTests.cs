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

  /* Simplest option
  Solution:
  - Template config at the level of the root folder parent
  
  Modules:
  - Instantiate a base module in a new folder
  - Make the changes specific to the module
  - Everything in the module copies over
   
  Sub modules:
  - Instantiate a base module in a new folder
  - Make the changes needed for the module
  - Make a template config file for the files added
  */
  
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
