using ControllerApi.Models;
using Microsoft.OpenApi.Models;
using ControllerApi;
using ControllerApi.Authentication;


bool useApiKeyAuthenticationFilter = false;
bool useApiKeyAuthMiddleware = false;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if(!useApiKeyAuthenticationFilter)
    builder.Services.AddControllers(); 
else // method 2 (filter): apply to every controller
    builder.Services.AddControllers(x => x.Filters.Add<ApiKeyAuthFilter>());

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


//builder.Services.AddScoped<ApiKeyAuthFilter>(); // method 3: allow using filters on controllers/methods

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if(useApiKeyAuthMiddleware)
    app.UseMiddleware<ApiKeyAuthMiddleware>(); // method 1 (middleware): use to apply apikey to every method (works with both controllers and minimal API)

//app.UseAuthorization();

app.MapControllers();

// using minimal API
app.MapGet("/miniget", () => WeatherForecast.Generate()).WithName("GetWeatherForecastMini");
app.MapGet("/", () => "Controller API");

app.Run();
