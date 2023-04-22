using ControllerApi.Models;
using Microsoft.OpenApi.Models;
using ControllerApi;
using ControllerApi.Authentication;


const bool USE_APIKEY_AUTH_MIDDLEWARE = false;
const bool USE_APIKEY_AUTH_FILTER = false;
const bool USE_APIKEY_SCOPED_AUTH_FILTER = true;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if(USE_APIKEY_AUTH_FILTER && !USE_APIKEY_SCOPED_AUTH_FILTER)
    builder.Services.AddControllers(x => x.Filters.Add<ApiKeyAuthFilter>()); // method 2a (filter): apply to every controller
else
    builder.Services.AddControllers(); 
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
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
    var requirement = new OpenApiSecurityRequirement
    {
        {scheme, new List<string>() }
    };
    c.AddSecurityRequirement(requirement);
});


if (!USE_APIKEY_AUTH_FILTER && USE_APIKEY_SCOPED_AUTH_FILTER)
    builder.Services.AddScoped<ApiKeyAuthFilter>(); // method 2b: allow using filters on controllers/methods

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if(USE_APIKEY_AUTH_MIDDLEWARE)
    app.UseMiddleware<ApiKeyAuthMiddleware>(); // method 1 (middleware): use to apply apikey to every method (works with both controllers and minimal API)

// app.UseAuthorization();

app.MapControllers();

// using minimal API
app.MapGet("/miniget", () => WeatherForecast.Generate()).WithName("GetWeatherForecastMini");
app.MapGet("/", () => "Controller API");

app.Run();
