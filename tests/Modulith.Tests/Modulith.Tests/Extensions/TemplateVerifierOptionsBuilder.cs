using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.TemplateEngine.Authoring.TemplateVerifier;

namespace Modulith.Tests.Extensions;

public class TemplateVerifierOptionsBuilder(string templateName)
{
  private readonly List<string> _templateArgs = [];

  private string       _templatePath = ".";
  private bool         _disableDiffTool;
  private List<string> _excludePaths       = [];
  private string       _snapshotsDirectory = ".";
  private bool         _ensureOutputDirectory;
  public  bool         DeleteEmptyOutputDirectory { get; private set; }
  public  bool         DeleteExclusions           { get; private set; }

  private ScrubbersDefinition _customScrubbers     = ScrubbersDefinition.Empty;
  private bool                _prependTemplateName = true;
  private string?             _outputDirectory;

  private static readonly Regex ProjectGuidRegex = new("\\s\"{[0-9a-fA-F]{8}[-]?[0-9a-fA-F]{4}[-]?[0-9a-fA-F]{4}[-]?[0-9a-fA-F]{4}[-]?[0-9a-fA-F]{12}");

  private readonly ScrubbersDefinition _guidScrubber = ScrubbersDefinition.Empty
    .AddScrubber((path, content) => {
      if (!Path.GetExtension(path).Equals(".sln", StringComparison.OrdinalIgnoreCase))
      {
        return;
      }

      var guids = ProjectGuidRegex.Matches(content.ToString());

      var items = guids
        .Distinct()
        .Select((guid, index) => (guid: guid.ToString(), index))
        .ToList();
      items
        .ForEach((item) => {
          var newGuid  = Guid.Empty.ToString().Replace("00", item.index <= 9 ? $"0{item.index}" : $"{item.index}");
          var oldValue = item.guid.Split("{")[1];
          content.Replace(oldValue, newGuid);
        });
    });

  public TemplateVerifierOptionsBuilder PrependTemplateNameToScenarioName(bool prependTemplateName)
  {
    _prependTemplateName = prependTemplateName;
    return this;
  }

  public TemplateVerifierOptionsBuilder WithArgs(IEnumerable<string>? templateArgs)
  {
    _templateArgs.AddRange(templateArgs ?? []);
    return this;
  }

  public TemplateVerifierOptionsBuilder WithTemplatePath(string templatePath)
  {
    _templatePath = templatePath;
    return this;
  }

  public TemplateVerifierOptionsBuilder DisableDiffTool(bool disableDiffTool = true)
  {
    _disableDiffTool = disableDiffTool;
    return this;
  }

  public TemplateVerifierOptionsBuilder ExcludeVerificationPaths(IEnumerable<string> excludePaths)
  {
    _excludePaths.AddRange(excludePaths);
    return this;
  }

  public TemplateVerifierOptionsBuilder WithSnapshotsDirectory(string outputDirectory)
  {
    if (!Directory.Exists(outputDirectory))
    {
      Directory.CreateDirectory(outputDirectory);
    }

    _snapshotsDirectory = outputDirectory;
    return this;
  }

  public TemplateVerifierOptionsBuilder WithOutputDirectory(string outputDirectory)
  {
    if (!Directory.Exists(outputDirectory))
    {
      Directory.CreateDirectory(outputDirectory);
    }

    _outputDirectory = outputDirectory;
    return this;
  }

  public TemplateVerifierOptionsBuilder KeepInstantiationInSnapshot([CallerMemberName] string? testName = null)
  {
    WithOutputDirectory($"{testName}._.instance");
    return this;
  }

  public TemplateVerifierOptionsBuilder EnsureEmptyOutputDirectory(bool ensureEmptyOutputDirectory = true)
  {
    _ensureOutputDirectory = ensureEmptyOutputDirectory;
    return this;
  }

  public TemplateVerifierOptionsBuilder DeletingReceivingDirectory(bool deleteEmptyOutputDirectory = true)
  {
    DeleteEmptyOutputDirectory = deleteEmptyOutputDirectory;
    return this;
  }

  public TemplateVerifierOptionsBuilder DeletingVerifyCompilationFiles(bool deleteExclusions = true)
  {
    DeleteExclusions = deleteExclusions;
    return this;
  }
  
  public TemplateVerifierOptionsBuilder WithCustomScrubbers(ScrubbersDefinition scrubbers)
  {
    _customScrubbers = scrubbers;
    return this;
  }

  public TemplateVerifierOptionsBuilder InstantiateWithoutVerification()
  {
    _excludePaths = ["**/*"];
    EnsureEmptyOutputDirectory(false);
    return this;
  }

  public TemplateVerifierOptions Build()
  {
    return new TemplateVerifierOptions(templateName)
      {
        TemplateSpecificArgs                   = _templateArgs,
        TemplatePath                           = _templatePath,
        DisableDiffTool                        = _disableDiffTool,
        VerificationExcludePatterns            = _excludePaths,
        OutputDirectory                        = _outputDirectory ?? Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()),
        EnsureEmptyOutputDirectory             = _ensureOutputDirectory,
        DoNotPrependTemplateNameToScenarioName = _prependTemplateName,
        DoNotAppendTemplateArgsToScenarioName  = true,
        SnapshotsDirectory                     = _snapshotsDirectory
      }
      .WithCustomScrubbers(_customScrubbers);
  }

  public TemplateVerifierOptionsBuilder WithDefaultOptions() =>
    DisableDiffTool()
      .WithCustomScrubbers(_guidScrubber)
      .ExcludeVerificationPaths(["**/*.DS_Store*"]);

}