using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class APIController : ControllerBase
    {
        protected IMediator _mediator;
        protected string accessToken = string.Empty;
        private readonly IHttpContextAccessor _contextAccessor;
        public string UserId;

        public APIController(IMediator mediator, IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _mediator = mediator;

            // Retrieve the access token from cookies  
            accessToken = _contextAccessor?.HttpContext?.Request.Cookies["talent-portal-accessToken"];

            // Validate and parse the JWT token safely  
            if (!string.IsNullOrEmpty(accessToken))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    var securityToken = tokenHandler.ReadJwtToken(accessToken);
                    UserId = securityToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value; // Safely retrieve the value  
                }
                catch (SecurityTokenException)
                {
                    UserId = null; // Invalidate UserId if token is invalid  
                }
                catch (Exception ex)
                {
                    // Handle unexpected exceptions here (optional)  
                    UserId = null;
                    // Log the exception or handle accordingly  
                }
            }
        }

        protected string GetActorEmail()
        {
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }
    }
}