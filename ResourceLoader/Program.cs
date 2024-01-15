using Microsoft.OpenApi.Models;
using ResourceLoader.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Resource loader API",
        Description = "An ASP.NET Core Web API for loading system's CPU and memory",
    });
});
builder.Services.AddSingleton<LoadSimulatorService>();
var app = builder.Build();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
app.Run();