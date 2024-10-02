using System.Runtime.CompilerServices;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.TemplateEngine.Authoring.TemplateVerifier;
using Microsoft.TemplateEngine.Utils;
using Modulith.Tests.SolutionWithUi;

namespace Modulith.Tests.Extensions;

public static class VerificationEngineExtensions
{
  private const string Modulith = "modulith";

  private static string  _codebase        = typeof(SolutionWithUiTests).Assembly.Location;
  private static string? _codeBaseRoot    = new FileInfo(_codebase).Directory?.Parent?.Parent?.Parent?.Parent?.Parent?.Parent?.FullName;
  private static string  _workingLocation = Path.Combine(_codeBaseRoot!, "working");

  public static async Task<TemplateVerifierOptions> TryExecute(this VerificationEngine engine, Func<TemplateVerifierOptionsBuilder, TemplateVerifierOptionsBuilder> optionsBuilder,
    [CallerMemberName] string? callerMethod = null,
    [CallerFilePath] string? callerFile = null)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(callerMethod);
    ArgumentException.ThrowIfNullOrWhiteSpace(callerFile);
    
    var parentFullName = new DirectoryInfo(callerFile).Parent?.FullName;
    ArgumentException.ThrowIfNullOrWhiteSpace(parentFullName);
    
    try
    {
      return await Execute(engine, optionsBuilder, callerMethod, Path.Combine(parentFullName, callerFile));
    }
    catch (TemplateVerificationException)
    {
      return optionsBuilder(GetVerificationOptions(callerMethod)).Build();
    }
  }

  public static async Task<TemplateVerifierOptions> Execute(
    this VerificationEngine engine,
    Func<TemplateVerifierOptionsBuilder,
      TemplateVerifierOptionsBuilder> optionsBuilder,
    [CallerMemberName] string? callerMethod = null,
    [CallerFilePath] string? callerFile = null)
  {
    var testLocation   = Path.Combine(new DirectoryInfo(callerFile).Parent.FullName);
    var defaultOptions = GetVerificationOptions(testLocation);
    var builder        = optionsBuilder(defaultOptions);
    var options        = builder.Build();

    if (builder.DeleteEmptyOutputDirectory)
    {
      ArgumentNullException.ThrowIfNull(options.OutputDirectory);
      DeleteDirectory(options.OutputDirectory);
    }

    RemoveVerifiedCompilationFiles(options, builder.DeleteExclusions);
    await engine.Execute(options, callerMethod: callerMethod!);
    
    BuildAndTestSolution(options);

    return options;
  }
  private static void BuildAndTestSolution(TemplateVerifierOptions options)
  {
    ArgumentNullException.ThrowIfNull(options.TemplateSpecificArgs);
    ArgumentNullException.ThrowIfNull(options.OutputDirectory);
    
    var solutionLocation= GetProjectLocation(options);

    Command.CreateDotNet("build", [solutionLocation])
      .WorkingDirectory(solutionLocation)
      .CaptureStdErr()
      .CaptureStdOut()
      .Execute()
      .Should()
      .Pass()
      .And.NotHaveStdErr();
    
    Command.CreateDotNet("test", [solutionLocation])
      .CaptureStdErr()
      .CaptureStdOut()
      .Execute()
      .Should()
      .Pass()
      .And.NotHaveStdErr();
  }
  private static string GetProjectLocation(TemplateVerifierOptions options)
  {

    var outputArg       = options.TemplateSpecificArgs.ToList().FindIndex(a => a is "-o" or "--output");
    var outputLocation  = options.TemplateSpecificArgs.ToList()[outputArg + 1];
    var projectLocation = Path.Combine(options.OutputDirectory, outputLocation);

    return FindSolutionFile(projectLocation);
  }
  private static string FindSolutionFile(string projectLocation)
  {
    var projectInfo      = new DirectoryInfo(projectLocation);
    var containsSolution = projectInfo.EnumerateFiles("*.sln").Any();
    if (containsSolution)
    {
      return $"/private{projectLocation}";
    }

    ArgumentException.ThrowIfNullOrWhiteSpace(projectInfo.Parent?.FullName);
    return FindSolutionFile(projectInfo.Parent.FullName);
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
        if (globPatterns.Exists(g => g.IsMatch(folder)))
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

  private static TemplateVerifierOptionsBuilder GetVerificationOptions(string testLocation)
  {
    return TemplateVerifierOptionsExtensions.ForTemplate(Modulith)
        .WithDefaultOptions()
        .WithTemplatePath(_workingLocation)
        .WithSnapshotsDirectory(testLocation)
      // .KeepInstantiationInSnapshot(testLocation)
      ;
  }
}