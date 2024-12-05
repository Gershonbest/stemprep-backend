using Microsoft.AspNetCore.Http;

namespace Application.Common
{
    public static class CookieHelper
    {
        public static void SetTokensInCookies(IHttpContextAccessor httpContextAccessor, string accessToken, string refreshToken)
        {
            var httpContext = httpContextAccessor?.HttpContext;

            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }

            // Define access token cookie options (30 minutes)
            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = false, // The cookie is accessible only by the server
                Expires = DateTime.UtcNow.AddMinutes(30), // Set expiration time for access token
                SameSite = SameSiteMode.Strict, // Restrict cookie to same-site requests
                Secure = true // Ensure HTTPS usage in production
            };

            // Define refresh token cookie options (1 day)
            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = false, // The cookie is accessible only by the server
                Expires = DateTime.UtcNow.AddHours(24), // Set expiration time for refresh token
                SameSite = SameSiteMode.Strict, // Restrict cookie to same-site requests
                Secure = true // Ensure HTTPS usage in production
            };

            // Append the cookies to the response
            httpContext.Response.Cookies.Append("AccessToken", accessToken, accessTokenOptions);
            httpContext.Response.Cookies.Append("RefreshToken", refreshToken, refreshTokenOptions);
        }
    }
}
