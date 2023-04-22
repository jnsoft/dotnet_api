using MiniApi.Authentication;
using Models;
using Common;
using Microsoft.OpenApi.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); // needed for minimal api:s
builder.Services.AddSwaggerGen(c => 
{
    c.AddSecurityDefinition("ApiKey", OpenApiSecurityDefinitions.ApiKeySecurityScheme());
    c.AddSecurityRequirement(OpenApiSecurityDefinitions.ApiKeySecurityRequirement());
});

//builder.Services.AddHttpsRedirection(options =>
//{
//    options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
//    options.HttpsPort = 443;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.MapGet("/", () => Results.Redirect("https://example.com", true, true));
//app.MapGet("/{*_}", (string _) => Results.Redirect("https://example.com", true, true));

// app.MapGet("/", () => Results.Redirect("/swagger"));

//app.UseEndpoints(endpoints =>
//{
    // redirect to one route
//    endpoints.Redirect("/", "/hello");
//});

app.Map("/ping", () => "pong");

var wgroup = app.MapGroup("weather").AddEndpointFilter<ApiKeyEndpointFilter>();
wgroup.MapGet("/weatherforecast", () =>
{
    return WeatherForecast.GenerateWeatherReport();
}) // .add
.WithName("GetWeatherForecast");

app.Run();

