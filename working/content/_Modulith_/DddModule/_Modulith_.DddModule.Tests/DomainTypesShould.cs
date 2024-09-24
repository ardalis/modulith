using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;

namespace _Modulith_.DddModule.Tests;

public class DomainTypesShould
{
  private static readonly Architecture Architecture =
    new ArchLoader()
      .LoadAssemblies(typeof(AssemblyInfo).Assembly)
      .Build();

  [Fact]
  public void NotDependOnApiTypes()
  {
    var domainTypes = ArchRuleDefinition.Types()
      .That()
      .ResideInNamespace("_Modulith_.DddModule.Domain.*", true)
      .And()
      .AreNot([typeof(AssemblyInfo), typeof(DddModuleServiceRegistrar)])
      .As("Domain types");

    var apiTypes = ArchRuleDefinition.Types()
      .That()
      .ResideInNamespace("_Modulith_.DddModule.Api.*", true)
      .As("Api types");

    var rule = domainTypes.Should().NotDependOnAny(apiTypes);

    rule.Check(Architecture);
  }

  [Fact]
  public void NotDependOnInfraTypes()
  {
    var domainTypes = ArchRuleDefinition.Types()
      .That()
      .ResideInNamespace("_Modulith_.DddModule.Domain.*", true)
      .And()
      .AreNot([typeof(AssemblyInfo), typeof(DddModuleServiceRegistrar)])
      .As("Domain types");

    var apiTypes = ArchRuleDefinition.Types()
      .That()
      .ResideInNamespace("_Modulith_.DddModule.Infrastructure.*", true)
      .As("Api types");

    var rule = domainTypes.Should().NotDependOnAny(apiTypes);

    rule.Check(Architecture);
  }
}
