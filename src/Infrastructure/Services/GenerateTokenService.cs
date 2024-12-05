using Microsoft.IdentityModel.Tokens;  
using System.IdentityModel.Tokens.Jwt;  
using System.Security.Claims;  
using System.Text;  
using Application.Interfaces;  

namespace Infrastructure.Services  
{  
    public class GenerateTokenService : IGenerateToken  
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
        

        public TokenResponse GenerateTokens(string userId, string email, string role)  
        {  
            var tokenHandler = new JwtSecurityTokenHandler();  
            var key = Encoding.ASCII.GetBytes(_key);  
            var refreshKey = Encoding.ASCII.GetBytes(_refreshKey);  

            // Generate Access Token (valid for 30 minutes)  
            var accessTokenDescriptor = new SecurityTokenDescriptor  
            {  
                Subject = new ClaimsIdentity(new[]  
                {  
                    new Claim(ClaimTypes.NameIdentifier, userId),  
                    new Claim(ClaimTypes.Email, email),  
                    new Claim(ClaimTypes.Role, role)  
                }),  
                Expires = DateTime.UtcNow.AddMinutes(30), // Access token expires in 30 minutes  
                Issuer = _issuer,  
                Audience = _audience,  
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)  
            };  
            var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);  
            var accessTokenString = tokenHandler.WriteToken(accessToken);  

            // Generate Refresh Token (valid for 1 day)  
            var refreshTokenDescriptor = new SecurityTokenDescriptor  
            {  
                Subject = new ClaimsIdentity(new[]  
                {  
                    new Claim(ClaimTypes.NameIdentifier, userId)  
                }),  
                Expires = DateTime.UtcNow.AddDays(1), // Refresh token expires in 1 day  
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

        public string GetEmailFromToken(ClaimsPrincipal user)  
        {  
            var emailClaim = user.FindFirst(ClaimTypes.Email)?.Value;  
            if (emailClaim == null)  
            {  
                throw new UnauthorizedAccessException("Email not found in token.");  
            }  

            return emailClaim;  
        }  

        public string GetIdFromToken(ClaimsPrincipal user)  
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