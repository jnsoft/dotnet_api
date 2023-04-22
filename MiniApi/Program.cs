using MiniApi.Authentication;
using MiniApi.Models;
using MiniApi.Common;
using Microsoft.OpenApi.Models;
using System.Net;

const bool USE_APIKEY_AUTH_MIDDLEWARE = true;

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

if(USE_APIKEY_AUTH_MIDDLEWARE)
    app.UseMiddleware<ApiKeyAuthMiddleware>();

//if (USE_APIKEY_AUTH_MIDDLEWARE)
    //app.UseAuthorization();

//app.MapGet("/", () => Results.Redirect("https://example.com", true, true));
//app.MapGet("/{*_}", (string _) => Results.Redirect("https://example.com", true, true));

// app.MapGet("/", () => Results.Redirect("/swagger"));

//app.UseEndpoints(endpoints =>
//{
    // redirect to one route
//    endpoints.Redirect("/", "/hello");
//});

/*
app.MapGet("/miniget", () => Models.WeatherForecast.Generate())
    .WithName("GetWeatherForecastMini");
    */

/*
var wgroup = app.MapGroup("weather").AddEndpointFilter<ApiKeyEndpointFilter>();
wgroup.MapGet("/weatherforecast", () =>
{
    return WeatherForecast.GenerateWeatherReport();
}) // .add
.WithName("GetWeatherForecast");
*/

app.MapGet("/ping", () => "pong");
app.Run();

