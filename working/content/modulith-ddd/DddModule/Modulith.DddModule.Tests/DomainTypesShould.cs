using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;

namespace Modulith.NewModule.Tests;

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
      .ResideInNamespace("Modulith.NewModule.Domain.*", useRegularExpressions: true)
      .And().AreNot([typeof(AssemblyInfo), typeof(NewModuleModuleHostApplicationBuilderExtensions)])
      .As("Domain types");
    
    var apiTypes = ArchRuleDefinition.Types()
      .That()
      .ResideInNamespace("Modulith.NewModule.Api.*", useRegularExpressions: true)
      .And().AreNot([typeof(AssemblyInfo), typeof(NewModuleModuleHostApplicationBuilderExtensions)])
      .As("Api types");

    var rule = domainTypes.Should().NotDependOnAny(apiTypes);

    rule.Check(Architecture);
  }
}
