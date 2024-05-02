using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Modulith.NewModule;
using Modulith.NewModule.UI;

#region Blazor Usings
using Modulith.UI.Pages;
using Modulith.UI;
using Modulith.Web.Components;
using MudBlazor.Services;
#endregion
using Modulith.Web;
using MudBlazor.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Call the method where you are registering services for each module:
// NewModuleModuleServiceRegistrar.ConfigureServices(builder);

// Or use the discover method below to try and find the services for your modules
builder.DiscoverAndRegisterModules();

builder.Services
  .AddAuthenticationJwtBearer(s =>
  {
    // TODO: Add dotnet secrets
    s.SigningKey = builder.Configuration["Auth:JwtSecret"];
  })
  .AddAuthorization()
  .SwaggerDocument()
  .AddFastEndpoints();

#region BlazorServices
// Add services to the container.
builder.Services.AddRazorComponents()
  .AddInteractiveServerComponents()
  .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();
builder.Services.RegisterNewModuleSpaServices();

#endregion

var app = builder.Build();

app.UseHttpsRedirection();


#region Blazor static files
app.UseStaticFiles();
#endregion

// Use FastEndpoints
app.UseAuthentication()
  .UseAuthorization()
  #region Blazor Middleware
  .UseRouting()
  .UseAntiforgery()
  #endregion
  .UseFastEndpoints()
  .UseSwaggerGen();

#region Blazor Component Middleware
var componentBuilder = app.MapRazorComponents<App>()
  .AddInteractiveServerRenderMode()
  .AddInteractiveWebAssemblyRenderMode()
  .AddAdditionalAssemblies(typeof(Counter).Assembly);
  
componentBuilder.AddAdditionalAssemblies(typeof(ModularComponent).Assembly);
#endregion

app.Run();

namespace Modulith.Web
{
  public partial class Program
  {
    
  }
}