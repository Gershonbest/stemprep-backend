using System.Security.Claims;  

namespace Application.Interfaces  
{  
    public interface IGenerateToken  
    {  
        TokenResponse GenerateTokens(string userId, string email, string role);  

        string GetEmailFromToken(ClaimsPrincipal user);  

        string GetIdFromToken(ClaimsPrincipal user);  
    }  
}  

public class TokenResponse  
{  
    public string AccessToken { get; set; }  
    public string RefreshToken { get; set; }  
}