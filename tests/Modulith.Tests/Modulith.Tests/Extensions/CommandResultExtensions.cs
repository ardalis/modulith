using Microsoft.DotNet.Cli.Utils;

namespace Modulith.Tests.Extensions;

public static class CommandResultExtensions
{
  public static CommandResultAssertions Should(this CommandResult commandResult) => new CommandResultAssertions(commandResult);
}
