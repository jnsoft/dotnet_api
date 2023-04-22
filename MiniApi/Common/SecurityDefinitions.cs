using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MiniApi.Common;
public static class OpenApiSecurityDefinitions
{
    public static OpenApiSecurityScheme ApiKeySecurityScheme()
    {
        return new OpenApiSecurityScheme 
        {
            Description = "The API key to access the API",
            Type = SecuritySchemeType.ApiKey,
            Name = "x-api-key",
            In = ParameterLocation.Header,
            Scheme = "ApiKeyScheme"
        };
    }

    public static OpenApiSecurityRequirement ApiKeySecurityRequirement() => new OpenApiSecurityRequirement {{apiKeySecuritySchemeForRequirement(), new List<string>() }};
    
    private static OpenApiSecurityScheme apiKeySecuritySchemeForRequirement()
    {
        return new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "ApiKey"
            },
            In = ParameterLocation.Header
        };
    }
   
}