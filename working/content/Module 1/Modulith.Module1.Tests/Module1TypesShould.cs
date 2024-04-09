using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Modulith.Module1.Tests;

public class Module1TypesShould
{
  private static readonly Architecture Architecture =
    new ArchLoader()
      .LoadAssemblies(typeof(AssemblyInfo).Assembly)
      .Build();

  [Fact]
  public void BeInternal()
  {
    var domainTypes = Types()
      .That()
      .ResideInNamespace("Modulith.Module1.*", useRegularExpressions: true)
      .And().AreNot([typeof(AssemblyInfo), typeof(Module1ModuleHostApplicationBuilderExtensions)])
      .As("Module types");

    var rule = domainTypes.Should().BeInternal();

    rule.Check(Architecture);
  }
}
