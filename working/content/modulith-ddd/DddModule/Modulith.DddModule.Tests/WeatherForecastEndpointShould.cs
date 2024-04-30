using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Modulith.NewModule.Api;

namespace Modulith.NewModule.Tests;

public class WeatherForecastEndpointShould(NewModuleFixture fixture) : TestBase<NewModuleFixture>
{
  [Fact]
  public async Task ReturnWeatherForecastDataAsync()
  {
    var call = await fixture.Client.GETAsync<WeatherForecastEndpoint, WeatherForecastResponse[]>();

    call.Response.EnsureSuccessStatusCode();
    call.Result.Should().HaveCount(5);
  }
}

public class NewModuleFixture : AppFixture<AssemblyInfo>
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
