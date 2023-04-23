using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using ControllerApi.Common;

const bool USE_APIKEY_AUTH_MIDDLEWARE = false;
const bool USE_APIKEY_AUTH_FILTER = false;
const bool USE_APIKEY_SCOPED_AUTH_FILTER = false;
const bool USE_JWT_AUTHENTICATION = true;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

#region Services

if (USE_JWT_AUTHENTICATION)
{
    // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = config["Authentication:JwtSettings:Issuer"],
            ValidAudience = config["Authentication:JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(config["Authentication:JwtSettings:Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(IdentityData.AdminUserPolicy, p =>
            p.RequireClaim(IdentityData.AdminUserClaim, "true"));
    });
}

if (USE_APIKEY_AUTH_FILTER && !USE_APIKEY_SCOPED_AUTH_FILTER)
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

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddSingleton<IItemsDatabase, ItemsDatabase>();

#endregion

var app = builder.Build();

#region Middleware


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if(USE_APIKEY_AUTH_MIDDLEWARE)
    app.UseMiddleware<ApiKeyAuthMiddleware>(); // method 1 (middleware): use to apply apikey to every method (works with both controllers and minimal API)

if(USE_JWT_AUTHENTICATION)
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.MapControllers();

// using minimal API
app.MapGet("/miniget", () => WeatherForecast.Generate()).WithName("GetWeatherForecastMini");
app.MapGet("/", () => "Controller API");
app.MapPost("/", () => Results.Ok())
    .RequireAuthorization(IdentityData.AdminUserPolicy);

#endregion

app.Run();
