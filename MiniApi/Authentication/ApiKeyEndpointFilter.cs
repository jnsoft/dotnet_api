namespace MiniApi.Authentication;

public class ApiKeyEndpointFilter : IEndpointFilter // need .net7
{
    private readonly IConfiguration _config;

    public ApiKeyEndpointFilter(IConfiguration configuration) => _config = configuration;

    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {

        if (!context.HttpContext.Request.Headers.TryGetValue("x-api-key", out var header_key))
            return TypedResults.Unauthorized();

        var api_key = _config.GetValue<string>("Authentication:ApiKey");

        if (!api_key.Equals(header_key))
            return TypedResults.Unauthorized();

        return await next(context);
    }
}
