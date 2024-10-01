using System.Runtime.CompilerServices;
using Microsoft.TemplateEngine.Authoring.TemplateVerifier;
using Microsoft.TemplateEngine.Utils;

namespace Modulith.Tests;

public static class VerificationEngineExtensions
{
  private const string Modulith = "modulith";


  private static string  _codebase        = typeof(SolutionTests).Assembly.Location;
  private static string? _codeBaseRoot    = new FileInfo(_codebase).Directory?.Parent?.Parent?.Parent?.Parent?.Parent?.Parent?.FullName;
  private static string  _workingLocation = Path.Combine(_codeBaseRoot!, "working");
  private static string  _outputLocation  = Path.Combine(_codeBaseRoot!, "tests", "Modulith.Tests", "Modulith.Tests", "Snapshots");

  public static async Task<TemplateVerifierOptions> TryExecute(this VerificationEngine engine, Func<TemplateVerifierOptionsBuilder, TemplateVerifierOptionsBuilder> optionsBuilder,
    [CallerMemberName] string? callerMethod = null)
  {
    try
    {
      return await Execute(engine, optionsBuilder, callerMethod);
    }
    catch (TemplateVerificationException)
    {
      return optionsBuilder(GetVerificationOptions(callerMethod)).Build();
    }
  }
  public static async Task<TemplateVerifierOptions> Execute(this VerificationEngine engine, Func<TemplateVerifierOptionsBuilder, TemplateVerifierOptionsBuilder> optionsBuilder, [CallerMemberName] string? callerMethod = null)
  {
    var builder = optionsBuilder(GetVerificationOptions(callerMethod));
    var options = builder.Build();

    if (builder.DeleteEmptyOutputDirectory)
    {
      ArgumentNullException.ThrowIfNull(options.OutputDirectory);
      DeleteDirectory(options.OutputDirectory);
    }

    RemoveVerifiedCompilationFiles(options, builder.DeleteExclusions);
    await engine.Execute(options, callerMethod: callerMethod!);
    
    return options;
  }
  private static void RemoveVerifiedCompilationFiles(TemplateVerifierOptions options, bool builderDeleteExclusions)
  {
    if (!builderDeleteExclusions)
    {
      return;
    }
    List<string> exclusions =
    [
      @"**/.idea",
      @"**/obj",
      @"**\obj",
      @"**/bin",
      @"**\bin"
    ];

    var folders = new List<string>();
    var globs   = exclusions.Select(e => Glob.Parse(e)).ToList();
    SearchFolders(options.SnapshotsDirectory, globs, folders);

    folders.ForEach(DeleteDirectory);
  }

  private static void SearchFolders(string currentDirectory, List<Glob> globPatterns, List<string> matchedFolders)
  {
    try
    {
      foreach (var folder in Directory.GetDirectories(currentDirectory))
      {
        // Check if the current folder matches the glob pattern
        if (globPatterns.Any(g => g.IsMatch(folder)))
        {
          matchedFolders.Add(folder);
        }

        // Recursively search in subfolders
        SearchFolders(folder, globPatterns, matchedFolders);
      }
    }
    catch (UnauthorizedAccessException)
    {
      // Handle permission issues by skipping directories that cannot be accessed
      Console.WriteLine($"Access denied to directory: {currentDirectory}");
    }
  }

  private static void DeleteDirectory(string location)
  {
    if (!Directory.Exists(location))
    {
      return;
    }
    var info = new DirectoryInfo(location);
    info.Delete(true);
  }

  private static TemplateVerifierOptionsBuilder GetVerificationOptions([CallerMemberName] string? testName = null)
  {
    return TemplateVerifierOptionsExtensions.ForTemplate(Modulith)
      .WithDefaultOptions()
      .WithTemplatePath(_workingLocation)
      .WithSnapshotsDirectory(Path.Combine(_outputLocation, testName!))
      .KeepInstantiationInSnapshot(testName);
  }
}
