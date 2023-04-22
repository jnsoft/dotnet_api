using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ControllerApi.Authentication;

public class ApiKeyAuthFilter : IAsyncAuthorizationFilter
{
    private readonly IConfiguration _config;

    public ApiKeyAuthFilter(IConfiguration config) => _config = config;
    

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var header_key))
        {
            context.Result = new UnauthorizedObjectResult(AuthConstants.ApiKeyMissingText);
            return Task.CompletedTask;
        }

        var api_key = _config.GetValue<string>(AuthConstants.ApiKeyConfigLocation);

        if (!api_key.Equals(header_key))
        {
            context.Result = new UnauthorizedObjectResult(AuthConstants.ApiKeyInvalidText);
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}
