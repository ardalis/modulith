using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Modulith.NewModule.Tests;

public class NewModuleTypesShould
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
      .ResideInNamespaceMatching("Modulith.NewModule.*")
      .And()
      .AreNot([typeof(AssemblyInfo), typeof(NewModuleModuleServiceRegistrar)])
      .And()
      .DoNotResideInNamespace("Modulith.NewModule.Data.Migrations")
      .As("Module types");

    var rule = domainTypes.Should().BeInternal();

    rule.Check(Architecture);
  }
}
