using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;

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
      .ResideInNamespace("Modulith.DddModule.Domain.*", useRegularExpressions: true)
      .And().AreNot([typeof(AssemblyInfo), typeof(DddModuleServiceRegistrar)])
      .As("Domain types");
    
    var apiTypes = ArchRuleDefinition.Types()
      .That()
      .ResideInNamespace("Modulith.DddModule.Api.*", useRegularExpressions: true)
      .As("Api types");

    var rule = domainTypes.Should().NotDependOnAny(apiTypes);

    rule.Check(Architecture);
  }
  
  [Fact]
  public void NotDependOnInfraTypes()
  {
    var domainTypes = ArchRuleDefinition.Types()
      .That()
      .ResideInNamespace("Modulith.DddModule.Domain.*", useRegularExpressions: true)
      .And().AreNot([typeof(AssemblyInfo), typeof(DddModuleServiceRegistrar)])
      .As("Domain types");
    
    var apiTypes = ArchRuleDefinition.Types()
      .That()
      .ResideInNamespace("Modulith.DddModule.Infrastructure.*", useRegularExpressions: true)
      .As("Api types");

    var rule = domainTypes.Should().NotDependOnAny(apiTypes);

    rule.Check(Architecture);
  }
}
