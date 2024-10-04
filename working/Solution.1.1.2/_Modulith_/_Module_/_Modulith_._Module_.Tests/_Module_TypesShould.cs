using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace _Modulith_._Module_.Tests;

public class _Module_TypesShould
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
      .ResideInNamespace("_Modulith_._Module_.*", true)
      .And()
      .AreNot([typeof(AssemblyInfo), typeof(_Module_ModuleServiceRegistrar)])
      .And()
      .DoNotResideInNamespace("_Modulith_._Module_.Data.Migrations")
      .As("Module types");

    var rule = domainTypes.Should().BeInternal();

    rule.Check(Architecture);
  }
}
