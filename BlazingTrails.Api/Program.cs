using BlazingTrails.Api.Persistence;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.FileProviders;
using Serilog;
using GlobalErrorHandling;
using System.Collections;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

Log.Information("Easy Logger!");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlazingTrailsContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("BlazingTrailsContext")));

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.Load("BlazingTrails.Shared"));

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();

app.UseMiddleware<SerilogMiddleware>();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Images")),
    RequestPath = new PathString("/images")
});



app.UseRouting();

app.MapControllers();
app.MapFallbackToFile("index.html");


//log envs
foreach(DictionaryEntry e in System.Environment.GetEnvironmentVariables())
{
    Log.Information(e.Key  + ":" + e.Value);
}


app.Run();



