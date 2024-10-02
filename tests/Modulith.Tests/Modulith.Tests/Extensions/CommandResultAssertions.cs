// Taken from: https://github.com/dotnet/sdk/blob/main/test/Microsoft.NET.TestFramework/Assertions/CommandResultAssertions.cs

using System.Text.RegularExpressions;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.DotNet.Cli.Utils;

namespace Modulith.Tests.Extensions;

public class CommandResultAssertions(CommandResult commandResult)
{

  public AndConstraint<CommandResultAssertions> ExitWith(int expectedExitCode)
  {
    Execute.Assertion.ForCondition(commandResult.ExitCode == expectedExitCode)
      .FailWith(AppendDiagnosticsTo($"Expected command to exit with {expectedExitCode} but it did not."));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> Pass()
  {
    Execute.Assertion.ForCondition(commandResult.ExitCode == 0)
      .FailWith(AppendDiagnosticsTo($"Expected command to pass but it did not."));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> Fail()
  {
    Execute.Assertion.ForCondition(commandResult.ExitCode != 0)
      .FailWith(AppendDiagnosticsTo($"Expected command to fail but it did not."));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> HaveStdOut()
  {
    Execute.Assertion.ForCondition(!string.IsNullOrEmpty(commandResult.StdOut))
      .FailWith(AppendDiagnosticsTo("Command did not output anything to stdout"));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> HaveStdOut(string expectedOutput)
  {
    Execute.Assertion.ForCondition(commandResult.StdOut.Equals(expectedOutput, StringComparison.Ordinal))
      .FailWith(AppendDiagnosticsTo($"Command did not output with Expected Output. Expected: {expectedOutput}"));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> HaveStdOutContaining(string pattern)
  {
    Execute.Assertion.ForCondition(commandResult.StdOut.Contains(pattern))
      .FailWith(AppendDiagnosticsTo($"The command output did not contain expected result: {pattern}{Environment.NewLine}"));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> HaveStdOutContaining(Func<string, bool> predicate, string description = "")
  {
    Execute.Assertion.ForCondition(predicate(commandResult.StdOut))
      .FailWith(AppendDiagnosticsTo($"The command output did not contain expected result: {description} {Environment.NewLine}"));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> NotHaveStdOutContaining(string pattern)
  {
    Execute.Assertion.ForCondition(!commandResult.StdOut.Contains(pattern))
      .FailWith(AppendDiagnosticsTo($"The command output contained a result it should not have contained: {pattern}{Environment.NewLine}"));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> HaveStdOutContainingIgnoreSpaces(string pattern)
  {
    string commandResultNoSpaces = commandResult.StdOut.Replace(" ", "");

    Execute.Assertion
      .ForCondition(commandResultNoSpaces.Contains(pattern))
      .FailWith(AppendDiagnosticsTo($"The command output did not contain expected result: {pattern}{Environment.NewLine}"));

    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> HaveStdOutContainingIgnoreCase(string pattern)
  {
    Execute.Assertion.ForCondition(commandResult.StdOut.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
      .FailWith(AppendDiagnosticsTo($"The command output did not contain expected result (ignoring case): {pattern}{Environment.NewLine}"));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> HaveStdOutMatching(string pattern, RegexOptions options = RegexOptions.None)
  {
    Execute.Assertion.ForCondition(Regex.Match(commandResult.StdOut, pattern, options).Success)
      .FailWith(AppendDiagnosticsTo($"Matching the command output failed. Pattern: {pattern}{Environment.NewLine}"));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> NotHaveStdOutMatching(string pattern, RegexOptions options = RegexOptions.None)
  {
    Execute.Assertion.ForCondition(!Regex.Match(commandResult.StdOut, pattern, options).Success)
      .FailWith(AppendDiagnosticsTo($"The command output matched a pattern it should not have. Pattern: {pattern}{Environment.NewLine}"));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> HaveStdErr()
  {
    Execute.Assertion.ForCondition(!string.IsNullOrEmpty(commandResult.StdErr))
      .FailWith(AppendDiagnosticsTo("Command did not output anything to stderr."));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> HaveStdErr(string expectedOutput)
  {
    Execute.Assertion.ForCondition(commandResult.StdErr.Equals(expectedOutput, StringComparison.Ordinal))
      .FailWith(AppendDiagnosticsTo($"Command did not output the expected output to StdErr.{Environment.NewLine}Expected: {expectedOutput}{Environment.NewLine}Actual:   {commandResult.StdErr}"));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> HaveStdErrContaining(string pattern)
  {
    Execute.Assertion.ForCondition(commandResult.StdErr.Contains(pattern))
      .FailWith(AppendDiagnosticsTo($"The command error output did not contain expected result: {pattern}{Environment.NewLine}"));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> HaveStdErrContainingOnce(string pattern)
  {
    var lines         = commandResult.StdErr.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
    var matchingLines = lines.Count(line => line.Contains(pattern));
    Execute.Assertion.ForCondition(matchingLines == 0)
      .FailWith(AppendDiagnosticsTo($"The command error output did not contain expected result: {pattern}{Environment.NewLine}"));
    Execute.Assertion.ForCondition(matchingLines != 1)
      .FailWith(AppendDiagnosticsTo($"The command error output was expected to contain the pattern '{pattern}' once, but found it {matchingLines} times.{Environment.NewLine}"));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> NotHaveStdErrContaining(string pattern)
  {
    Execute.Assertion.ForCondition(!commandResult.StdErr.Contains(pattern))
      .FailWith(AppendDiagnosticsTo($"The command error output contained a result it should not have contained: {pattern}{Environment.NewLine}"));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> HaveStdErrMatching(string pattern, RegexOptions options = RegexOptions.None)
  {
    Execute.Assertion.ForCondition(Regex.Match(commandResult.StdErr, pattern, options).Success)
      .FailWith(AppendDiagnosticsTo($"Matching the command error output failed. Pattern: {pattern}{Environment.NewLine}"));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> NotHaveStdOut()
  {
    Execute.Assertion.ForCondition(string.IsNullOrEmpty(commandResult.StdOut))
      .FailWith(AppendDiagnosticsTo($"Expected command to not output to stdout but it was not:"));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> NotHaveStdErr()
  {
    Execute.Assertion.ForCondition(string.IsNullOrEmpty(commandResult.StdErr))
      .FailWith(AppendDiagnosticsTo("Expected command to not output to stderr but it was not:"));
    return new AndConstraint<CommandResultAssertions>(this);
  }

  private string AppendDiagnosticsTo(string s)
  {
    return s                                                                           + $"{Environment.NewLine}" +
           $"File Name: {commandResult.StartInfo?.FileName}{Environment.NewLine}"     +
           $"Arguments: {commandResult.StartInfo?.Arguments}{Environment.NewLine}"    +
           $"Exit Code: {commandResult.ExitCode}{Environment.NewLine}"                +
           $"StdOut:{Environment.NewLine}{commandResult.StdOut}{Environment.NewLine}" +
           $"StdErr:{Environment.NewLine}{commandResult.StdErr}{Environment.NewLine}";
  }

  public AndConstraint<CommandResultAssertions> HaveSkippedProjectCompilation(string skippedProject, string frameworkFullName)
  {
    commandResult.StdOut.Should().Contain($"Project {skippedProject} ({frameworkFullName}) was previously compiled. Skipping compilation.");

    return new AndConstraint<CommandResultAssertions>(this);
  }

  public AndConstraint<CommandResultAssertions> HaveCompiledProject(string compiledProject, string frameworkFullName)
  {
    commandResult.StdOut.Should().Contain($"Project {compiledProject} ({frameworkFullName}) will be compiled");

    return new AndConstraint<CommandResultAssertions>(this);
  }
}
