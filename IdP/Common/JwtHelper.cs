using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text;

namespace IdP.Common;

public static class JwtHelper
{
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(1);
    
// For debug purposes, never actually return signing key.
public static string GenerateJwtToken(TokenGenerationRequest req, string issuer, byte[] key)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        List<Claim> claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new(JwtRegisteredClaimNames.Sub, req.Email),
        new(JwtRegisteredClaimNames.Email, req.Email),
        new("userid", req.UserId.ToString())
    };

        foreach (var c in req.CustomClaims)
        {
            JsonElement el = (JsonElement)c.Value;
            string kind = el.ValueKind switch
            {
                JsonValueKind.True => ClaimValueTypes.Boolean,
                JsonValueKind.False => ClaimValueTypes.Boolean,
                JsonValueKind.Number => ClaimValueTypes.Double,
                _ => ClaimValueTypes.String
            };
            Claim claim = new Claim(c.Key, c.Value.ToString()!, kind);
            claims.Add(claim);
        }

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenLifetime),
            Issuer = issuer,
            Audience = req.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        key.Clear();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }
}
