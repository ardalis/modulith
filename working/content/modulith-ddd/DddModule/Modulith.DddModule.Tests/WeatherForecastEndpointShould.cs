using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Modulith.DddModule.Api;

namespace Modulith.DddModule.Tests;

public class WeatherForecastEndpointShould(DddModuleFixture fixture) : TestBase<DddModuleFixture>
{
  [Fact]
  public async Task ReturnWeatherForecastDataAsync()
  {
    var call = await fixture.Client.GETAsync<WeatherForecastEndpoint, WeatherForecastResponse[]>();

    call.Response.EnsureSuccessStatusCode();
    call.Result.Should().HaveCount(5);
  }
}

public class DddModuleFixture : AppFixture<AssemblyInfo>
{
  protected override async Task SetupAsync()
  {
    Client = CreateClient();

    await base.SetupAsync();
  }

  protected override async Task TearDownAsync()
  {
    Client.Dispose();
    await base.TearDownAsync();
  }
}
