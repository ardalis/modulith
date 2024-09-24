using System.Text.RegularExpressions;
using Microsoft.TemplateEngine.Authoring.TemplateVerifier;
using Microsoft.TemplateEngine.Utils;
using Xunit.Abstractions;

namespace Modulith.Tests;

public abstract class TestBase(ITestOutputHelper output)
{

  protected readonly      VerificationEngine Engine    = new(output.BuildLogger());
  private static readonly Regex              GuidRegex = new("[(]?[0-9a-fA-F]{8}[-]?[0-9a-fA-F]{4}[-]?[0-9a-fA-F]{4}[-]?[0-9a-fA-F]{4}[-]?[0-9a-fA-F]{12}[)}]?");
  protected const         string             Modulith  = "modulith";

  protected readonly ScrubbersDefinition GuidScrubber = ScrubbersDefinition.Empty.AddScrubber(line => {

    var matches = GuidRegex.Matches(line.ToString());

    matches
      .ForEach(match =>
        line.Replace(match.ToString(), Guid.Empty.ToString())
      );
  }, "sln");

  private static   string  _codebase        = typeof(SolutionTests).Assembly.Location;
  private static   string? _codeBaseRoot    = new FileInfo(_codebase).Directory?.Parent?.Parent?.Parent?.Parent?.Parent?.Parent?.FullName;
  private readonly string  _workingLocation = Path.Combine(_codeBaseRoot!, "working");
  protected static string  OutputLocation   = Path.Combine(_codeBaseRoot!, "tests", "Modulith.Tests", "Modulith.Tests", "Snapshots", "output");

  protected TemplateVerifierOptions WithVerificationOptions(IEnumerable<string>? templateArgs, string? customOutput = null)
  {
    TemplateVerifierOptions options = new(templateName: Modulith)
    {
      TemplateSpecificArgs        = templateArgs,
      TemplatePath                = _workingLocation,
      VerificationExcludePatterns = ["**/*.DS_Store*"],
      OutputDirectory             = customOutput,
      EnsureEmptyOutputDirectory  = false,
    };
    return options.WithCustomScrubbers(GuidScrubber);
  }

  protected TemplateVerifierOptions WithoutVerificationOptions(IEnumerable<string>? templateArgs, bool disableDiffTool = false)
  {
    TemplateVerifierOptions options = new(templateName: Modulith)
    {
      TemplateSpecificArgs        = templateArgs,
      TemplatePath                = _workingLocation,
      DisableDiffTool             = disableDiffTool,
      VerificationExcludePatterns = ["**/*"],
      OutputDirectory             = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()),
      EnsureEmptyOutputDirectory  = false,
    };
    return options.WithCustomScrubbers(GuidScrubber);
  }
}
