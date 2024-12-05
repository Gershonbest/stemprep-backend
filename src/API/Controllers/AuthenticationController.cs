using Application.Auth.Commands;  
using MediatR;  
using Microsoft.AspNetCore.Mvc;  

namespace API.Controllers  
{  
    [ApiController]  
    [Route("api/auth")]  
    public class AuthenticationController : ControllerBase 
    {  
        private readonly IMediator _mediator;  

        public AuthenticationController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {  
            _mediator = mediator;  
        }

        [HttpPost("student/register")]
        public async Task<IActionResult> Register(RegisterStudentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("admin/register")]
        public async Task<IActionResult> Register(RegisterAdminCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("instructor/register")]
        public async Task<IActionResult> Register(RegisterInstructorCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        [HttpPost("confirm-registration")]  
        public async Task<IActionResult> ConfirmRegistration(ConfirmRegistrationCommand command)  
        {  
            return Ok(await _mediator.Send(command));  
        }  

        [HttpPost("login")]  
        public async Task<IActionResult> Login(LoginUserCommand command)
        {

            var res = await _mediator.Send(command);
            var token = (TokenResponse)res?.Entity;
            if (token != null)
            {
                HttpContext.Response.Cookies.Append("talent-portal-accessToken", token.AccessToken);
                HttpContext.Response.Cookies.Append("talent-portal-refreshToken", token.RefreshToken);
            }
            
            return Ok(res);
        }  

        [HttpPost("forgot-password")]  
        public async Task<IActionResult> ForgetPassword(ForgotPasswordCommand command)  
        {  
            return Ok(await _mediator.Send(command));  
        }  
        
        [HttpPost("verify-reset-code")]  
        public async Task<IActionResult> VerifyResetCode(VerifyForgotPasswordCodeCommand command)  
        {  
            return Ok(await _mediator.Send(command));  
        }  

        [HttpPost("reset-password")]  
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)  
        {  
            return Ok(await _mediator.Send(command));  
        }  
    }  
}


