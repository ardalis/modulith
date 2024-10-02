using Microsoft.TemplateEngine.Authoring.TemplateVerifier;
using Xunit.Abstractions;

namespace Modulith.Tests;

public abstract class TestBase(ITestOutputHelper output)
{
  protected readonly VerificationEngine Engine   = new(output.BuildLogger());
}