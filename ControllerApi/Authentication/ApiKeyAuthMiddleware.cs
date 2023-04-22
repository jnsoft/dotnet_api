namespace ControllerApi.Authentication
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        } 

        public async Task InvokeAsync(HttpContext context)
        {
            if(!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var header_key))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(AuthConstants.ApiKeyMissingText);
                return;
            }

            var api_key = _config.GetValue<string>(AuthConstants.ApiKeyConfigLocation);

            if(!api_key.Equals(header_key))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(AuthConstants.ApiKeyInvalidText);
                return;
            }

            await _next(context);
        }
    }
}
