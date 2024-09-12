using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
#if (withui)
using Modulith.UI;
using MudBlazor.Services;
#endif
using Modulith.Web;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Call the method where you are registering services for each module:
// NewModuleModuleServiceRegistrar.ConfigureServices(builder.Services, builder.Configuration);

// Or use the discover method below to try and find the services for your modules`
builder.Services.DiscoverAndRegisterModules();

#if (withui)
builder.Services.AddBlazorAssemblyDiscovery();
#endif

builder.Services
  .AddAuthenticationJwtBearer(s => {
    // TODO: Add dotnet secrets
    s.SigningKey = builder.Configuration["Auth:JwtSecret"];
  })
  .AddAuthorization()
  .SwaggerDocument()
  .AddFastEndpoints();

#if (withui)
// Add services to the container.
builder.Services.AddRazorComponents()
  .AddInteractiveServerComponents()
  .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();
#endif
var app = builder.Build();

app.UseHttpsRedirection();
#if (withui)
app.UseStaticFiles()
  .UseWebAssemblyDebugging();
#endif

// Use FastEndpoints
app.UseAuthentication()
  .UseAuthorization()
#if (withui)
  .UseRouting()
  .UseAntiforgery()
#endif
  .UseFastEndpoints()
  .UseSwaggerGen();

#if (withui)
app.AddBlazorModulesAdditionalAssemblies();
#endif

app.Run();

namespace Modulith.Web
{
  public partial class Program;
}