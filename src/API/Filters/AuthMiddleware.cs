using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthMiddleware> _logger;
    private readonly string _jwtSecret; // Inject this if necessary

    public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        // _jwtSecret = jwtSecret;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var accessToken = httpContext.Request.Cookies["AccessToken"];

        if (string.IsNullOrEmpty(accessToken))
        {
            _logger.LogWarning("Access token is missing.");
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsync("Unauthorized: Missing access token.");
            return;
        }

        if (!ValidateToken(accessToken))
        {
            _logger.LogWarning("Invalid or expired access token.");
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsync("Unauthorized: Invalid or expired access token.");
            return;
        }

        _logger.LogInformation("Access token is valid.");
        await _next(httpContext); // Proceed if token is valid
    }

    private bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return validatedToken != null;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Token validation failed: {ex.Message}");
            return false;
        }
    }
}
