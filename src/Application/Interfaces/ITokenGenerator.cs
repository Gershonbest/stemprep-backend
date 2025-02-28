using System.Security.Claims;  

namespace Application.Interfaces  
{  
    public interface ITokenGenerator  
    {  
        TokenResponse GenerateTokens(string userId, string email, string role, Guid guid);  

        string GetEmailFromToken(ClaimsPrincipal user);  

        string GetOwnerIdFromToken(ClaimsPrincipal user);  
    }  
}  

public class TokenResponse  
{  
    public string AccessToken { get; set; }  
    public string RefreshToken { get; set; }  
}