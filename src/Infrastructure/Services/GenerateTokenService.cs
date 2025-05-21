using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;

namespace Infrastructure.Services
{
    public class GenerateTokenService : ITokenGenerator
    {
        private readonly string _key; // Key for signing access tokens  
        private readonly string _issuer; // Issuer of the tokens  
        private readonly string _audience; // Audience for the tokens  
        private readonly string _refreshKey; // Refresh token secret key  

        public GenerateTokenService(string key, string refreshKey, string issuer, string audience)
        {
            _key = key;
            _refreshKey = refreshKey;
            _issuer = issuer;
            _audience = audience;

        }

        public TokenResponse GenerateTokens(string userName, string email, string role, Guid guid, bool isOnboarded)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);
            var refreshKey = Encoding.ASCII.GetBytes(_refreshKey);

            // Generate Access Token (valid for 30 minutes)  
            var accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, guid.ToString()),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Name, userName),
                    new Claim("IsOnboarded", isOnboarded.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Access token expires in 2Hrs 
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);
            var accessTokenString = tokenHandler.WriteToken(accessToken);

            // Generate Refresh Token (valid for 30 days)  
            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, guid.ToString()),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddDays(30), // Refresh token expires in 30 days  
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(refreshKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);
            var refreshTokenString = tokenHandler.WriteToken(refreshToken);

            // Return both tokens  
            return new TokenResponse
            {
                AccessToken = accessTokenString,
                RefreshToken = refreshTokenString
            };
        }

        public (string, string, string, string) ExtractTokenInfo(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);
            var refreshKey = Encoding.ASCII.GetBytes(_refreshKey);


            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(refreshKey),
                ValidateIssuer = true, // Set these according to your validation needs
                ValidateAudience = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                // Allow for some clock skew
                ClockSkew = TimeSpan.FromMinutes(2)
            };

            // Validate token and retrieve ClaimsPrincipal
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            // Extract claims using their claim types. Adjust the claim types as needed.
            var guidClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var emailClaim = principal.FindFirst(ClaimTypes.Email)?.Value;
            var roleClaim = principal.FindFirst(ClaimTypes.Role)?.Value;
            var nameClaim = principal.FindFirst(ClaimTypes.Name)?.Value;

            return (guidClaim, emailClaim, roleClaim, nameClaim);
        }

        public string GetEmailFromToken(ClaimsPrincipal user)
        {
            var emailClaim = user.FindFirst(ClaimTypes.Email)?.Value;
            if (emailClaim == null)
            {
                throw new UnauthorizedAccessException("Email not found in token.");
            }

            return emailClaim;
        }

        public string GetOwnerIdFromToken(ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }

            return idClaim;
        }
    }
}