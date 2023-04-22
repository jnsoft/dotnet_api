namespace MiniApi.Authentication;
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
            // Check if header exists
            if(!context.Request.Headers.TryGetValue(ApiKeyAuthConstants.ApiKeyHeaderName, out var header_key))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(ApiKeyAuthConstants.ApiKeyMissingText);
                return;
            }

            IConfigurationSection sec = _config.GetSection("Authentication");
            bool test = _config.GetSection("Authentication").Exists();
            string test2 = _config[ApiKeyAuthConstants.ApiKeyConfigSection];

            var root = (IConfigurationRoot)_config;
            string debugView = root.GetDebugView();
 

            // Get key from configuration
            string api_key = _config.GetValue<string>(ApiKeyAuthConstants.ApiKeyConfigSection) ?? "";

            // Check if key matches
            if(string.IsNullOrWhiteSpace(api_key) || !api_key.Equals(header_key))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(ApiKeyAuthConstants.ApiKeyInvalidText);
                return;
            }

            // Move on to the next step in pipeline
            await _next(context);
        }
    }