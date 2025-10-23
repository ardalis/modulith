using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;

namespace Modulith.DddModule.Tests;

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
      .ResideInNamespaceMatching("Modulith.DddModule.Domain.*")
      .And()
      .AreNot([typeof(AssemblyInfo), typeof(DddModuleServiceRegistrar)])
      .As("Domain types");

    var apiTypes = ArchRuleDefinition.Types()
      .That()
      .ResideInNamespaceMatching("Modulith.DddModule.Api.*")
      .As("Api types");

    var rule = domainTypes.Should().NotDependOnAny(apiTypes);

    rule.Check(Architecture);
  }

  [Fact]
  public void NotDependOnInfraTypes()
  {
    var domainTypes = ArchRuleDefinition.Types()
      .That()
      .ResideInNamespaceMatching("Modulith.DddModule.Domain.*")
      .And()
      .AreNot([typeof(AssemblyInfo), typeof(DddModuleServiceRegistrar)])
      .As("Domain types");

    var apiTypes = ArchRuleDefinition.Types()
      .That()
      .ResideInNamespaceMatching("Modulith.DddModule.Infrastructure.*")
      .As("Api types");

    var rule = domainTypes.Should().NotDependOnAny(apiTypes);

    rule.Check(Architecture);
  }
}
