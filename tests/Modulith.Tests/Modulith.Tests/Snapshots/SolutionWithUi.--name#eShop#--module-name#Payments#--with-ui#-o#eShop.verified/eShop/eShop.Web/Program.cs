using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using eShop.UI;
using MudBlazor.Services;
using eShop.Web;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Call the method where you are registering services for each module:
// PaymentsModuleServiceRegistrar.ConfigureServices(builder);

// Or use the discover method below to try and find the services for your modules
builder.Services.DiscoverAndRegisterModules();

builder.Services.AddBlazorAssemblyDiscovery();

builder.Services
  .AddAuthenticationJwtBearer(s => {
    // TODO: Add dotnet secrets
    s.SigningKey = builder.Configuration["Auth:JwtSecret"];
  })
  .AddAuthorization()
  .SwaggerDocument()
  .AddFastEndpoints();

// Add services to the container.
builder.Services.AddRazorComponents()
  .AddInteractiveServerComponents()
  .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles()
  .UseWebAssemblyDebugging();

// Use FastEndpoints
app.UseAuthentication()
  .UseAuthorization()
  .UseRouting()
  .UseAntiforgery()
  .UseFastEndpoints()
  .UseSwaggerGen();

app.AddBlazorModulesAdditionalAssemblies();

app.Run();

namespace eShop.Web
{
  public partial class Program;
}