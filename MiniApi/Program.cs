using MiniApi.Authentication;
using MiniApi.Models;
using MiniApi.Common;
using Microsoft.OpenApi.Models;
using System.Net;
using Microsoft.Extensions.Logging.Console;

const bool USE_APIKEY_AUTH_MIDDLEWARE = false;
const bool USE_APIKEY_AUTH_ENDPOINT_FILTER = false;

var builder = WebApplication.CreateBuilder(args);
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Services.AddLogging(logging => logging.AddConsole());

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

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if(USE_APIKEY_AUTH_MIDDLEWARE)
    app.UseMiddleware<ApiKeyAuthMiddleware>();

//ILogger <LogTest> logger = app.Services.GetRequiredService<ILogger<LogTest>>();
//var test = new LogTest(logger);


//if (USE_APIKEY_AUTH_MIDDLEWARE)
    app.UseAuthorization();

//app.MapGet("/", () => Results.Redirect("https://example.com", true, true));
//app.MapGet("/{*_}", (string _) => Results.Redirect("https://example.com", true, true));

// app.MapGet("/", () => Results.Redirect("/swagger"));



RouteGroupBuilder wgroup;

if (USE_APIKEY_AUTH_ENDPOINT_FILTER)
    wgroup = app.MapGroup("weather").AddEndpointFilter<ApiKeyEndpointFilter>();
else
    wgroup = app.MapGroup("weather");

wgroup.MapGet("/weatherforecast", () =>
{
    return WeatherForecast.GenerateWeatherReport();
}).WithName("GetWeatherForecast");


app.MapGet("/", () => "Minimal API");

if(USE_APIKEY_AUTH_ENDPOINT_FILTER)
    app.MapGet("/ping", () => "pong")
        .AddEndpointFilter<ApiKeyEndpointFilter>();
else
    app.MapGet("/ping", () => "pong");
    

app.Run();

