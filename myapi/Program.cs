using myapi.Authentication;
using Models;
using Common;
using Microsoft.OpenApi.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); // needed for minimal api:s
builder.Services.AddSwaggerGen(c => // OpenApiSecurityDefinitions.ApiKeyDefinition());
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "The API key to access the API",
        Type = SecuritySchemeType.ApiKey,
        Name = "x-api-key",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requiriment = new OpenApiSecurityRequirement
    {
        {scheme, new List<string>() }
    };
    c.AddSecurityRequirement(requiriment);
});

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
    options.HttpsPort = 443;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Redirect("https://example.com", true, true));
app.MapGet("/{*_}", (string _) => Results.Redirect("https://example.com", true, true));

app.MapGet("/", () => Results.Redirect("/swagger"));

//app.UseEndpoints(endpoints =>
//{
    // redirect to one route
//    endpoints.Redirect("/", "/hello");
//});

var wgroup = app.MapGroup("weather").AddEndpointFilter<ApiKeyEndpointFilter>();

wgroup.MapGet("/weatherforecast", () =>
{
    return WeatherForecast.GenerateWeatherReport();
})//.add
.WithName("GetWeatherForecast");

app.Run();

