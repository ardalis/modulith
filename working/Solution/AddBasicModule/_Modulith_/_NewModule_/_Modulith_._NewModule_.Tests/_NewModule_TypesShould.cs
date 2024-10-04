using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace _Modulith_._NewModule_.Tests;

public class _NewModule_TypesShould
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
      .ResideInNamespace("_Modulith_._NewModule_.*", true)
      .And()
      .AreNot([typeof(AssemblyInfo), typeof(_NewModule_ModuleServiceRegistrar)])
      .As("Module types");

    var rule = domainTypes.Should().BeInternal();

    rule.Check(Architecture);
  }
}
