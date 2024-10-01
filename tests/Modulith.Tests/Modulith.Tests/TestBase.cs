using System.Runtime.CompilerServices;
using Microsoft.TemplateEngine.Authoring.TemplateVerifier;
using Xunit.Abstractions;

namespace Modulith.Tests;

public abstract class TestBase(ITestOutputHelper output)
{

  protected readonly VerificationEngine Engine   = new(output.BuildLogger());
  protected const    string             Modulith = "modulith";


  private static   string  _codebase        = typeof(SolutionTests).Assembly.Location;
  private static   string? _codeBaseRoot    = new FileInfo(_codebase).Directory?.Parent?.Parent?.Parent?.Parent?.Parent?.Parent?.FullName;
  private readonly string  _workingLocation = Path.Combine(_codeBaseRoot!, "working");
  protected static string  OutputLocation   = Path.Combine(_codeBaseRoot!, "tests", "Modulith.Tests", "Modulith.Tests", "Snapshots");

  protected TemplateVerifierOptionsBuilder GetVerificationOptions([CallerMemberName] string? testName = null)
  {
    return TemplateVerifierOptionsExtensions
      .ForTemplate(Modulith)
      .WithDefaultOptions()
      .WithTemplatePath(_workingLocation)
      .WithSnapshotsDirectory(Path.Combine(OutputLocation,  testName!))
      .KeepInstantiationInSnapshot(testName);
  }
}