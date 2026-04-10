using System.Security.Claims;

using FirebaseAdmin.Auth;
using Microsoft.IdentityModel.Tokens;


namespace Bike2Beans.Infrastructure.Security;

public class FirebaseTokenHandler : TokenHandler
{
    public override async Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationPraneters)
    {
        try
        {
            var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token, checkRevoked: true);
            var claims = new List<Claim>();
            var uid = decoded.Uid;
            claims.Add(new Claim("sub", uid));
            claims.Add(new Claim("user_id", uid));

            if (decoded.Claims.TryGetValue("email", out var emailObj) && emailObj is string email && !string.IsNullOrEmpty(email))
            {
                claims.Add(new Claim(ClaimTypes.Email, email));
                claims.Add(new Claim("emial", email));
            }
            if (decoded.Claims.TryGetValue("name", out var nameObj) && nameObj is string name && !string.IsNullOrEmpty(name))
            {
                claims.Add(new Claim(ClaimTypes.Name, name));
                claims.Add(new Claim("name", name));
            }
            // handle support admin claims
            if (decoded.Claims.TryGetValue("support_admin", out var supportadminObj))
            {
                var isSupportAdmin = supportadminObj switch
                {
                    bool b => b,
                    string s => s.Equals("true", StringComparison.OrdinalIgnoreCase),
                    _ => false
                };
                if (isSupportAdmin)
                {
                    claims.Add(new Claim("support_admin", "true"));
                }

            }

            foreach (var kvp in decoded.Claims)
            {
                if (kvp.Value is string s && !claims.Any(c => c.Type == kvp.Key))
                {
                    claims.Add(new Claim(kvp.Key, s));
                }
            }

            var identity = new ClaimsIdentity(claims, "Bearer");
            return new TokenValidationResult
            {
                IsValid = true,
                ClaimsIdentity = identity
            };
        }
        catch (Exception ex)
        {
            return new TokenValidationResult
            {
                IsValid = false,
                Exception = ex
            };
        }
    }
}