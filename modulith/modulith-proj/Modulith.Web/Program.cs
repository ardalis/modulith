using FastEndpoints;
using Modulith.NewModule;
using FastEndpoints.Security;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
  .AddAuthenticationJwtBearer(s =>
  {
    // TODO: Add dotnet secrets
    s.SigningKey = builder.Configuration["Auth:JwtSecret"];
  })
  .AddAuthorization()
  .SwaggerDocument()
  .AddFastEndpoints();

// Add module services
builder.AddNewModuleServices();

var app = builder.Build();

app.UseHttpsRedirection();

// Use FastEndpoints
app.UseAuthentication()
  .UseAuthorization()
  .UseFastEndpoints()
  .UseSwaggerGen();

app.Run();


