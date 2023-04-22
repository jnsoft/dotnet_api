namespace MiniApi.Authentication
{
    internal static class ApiKeyAuthConstants
    {
        public const string ApiKeyHeaderName = "X-Api-Key";
        public const string ApiKeyConfigSection = "Authentication:ApiKey";
        public const string ApiKeyMissingText = "API key missing";
        public const string ApiKeyInvalidText = "Invalid API key";
    }
}
