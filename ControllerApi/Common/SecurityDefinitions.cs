using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common;
public static class OpenApiSecurityDefinitions
{
    public static SwaggerGenOptions ApiKeyDefinition()
    {
        SwaggerGenOptions c = new SwaggerGenOptions();
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
        return c;
    }
}