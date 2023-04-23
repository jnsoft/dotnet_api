using System.Security.Claims;

namespace IdP
{
    public class TokenGenerationRequest
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = "";

        public string Audience { get; set; } = "";

        public List<CustomClaim> CustomClaims { get; set; } = new List<CustomClaim>();
    }

    public record CustomClaim(string Key, object Value);
}





