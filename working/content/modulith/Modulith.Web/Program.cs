using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
#if (WithUi)
using Modulith.NewModule.UI;
using Modulith.UI;
using Modulith.UI.Pages;
using Modulith.Web.Components;
using MudBlazor.Services;
#endif
using Modulith.Web;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Call the method where you are registering services for each module:
// NewModuleModuleServiceRegistrar.ConfigureServices(builder);

// Or use the discover method below to try and find the services for your modules
builder.DiscoverAndRegisterModules();

#if (WithUi)
builder.Services.AddBlazorAssemblyDiscovery();
#endif

builder.Services
  .AddAuthenticationJwtBearer(s =>
  {
    // TODO: Add dotnet secrets
    s.SigningKey = builder.Configuration["Auth:JwtSecret"];
  })
  .AddAuthorization()
  .SwaggerDocument()
  .AddFastEndpoints();

#if (WithUi)
// Add services to the container.
builder.Services.AddRazorComponents()
  .AddInteractiveServerComponents()
  .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();
#endif
var app = builder.Build();

app.UseHttpsRedirection();
#if (WithUi)
app.UseStaticFiles()
  .UseWebAssemblyDebugging();
#endif

// Use FastEndpoints
app.UseAuthentication()
  .UseAuthorization()
#if (WithUi)
  .UseRouting()
  .UseAntiforgery()
#endif
  .UseFastEndpoints()
  .UseSwaggerGen();

#if (WithUi)
app.AddBlazorModulesAdditionalAssemblies();
#endif

app.Run();

namespace Modulith.Web
{
  public partial class Program
  {
    
  }
}