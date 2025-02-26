using Application.Auth.Commands;
using Application.Parents.Queries;
using MediatR;  
using Microsoft.AspNetCore.Mvc;  

namespace API.Controllers  
{  
    [ApiController]  
    [Route("api/auth")]  
    public class AuthenticationController(IMediator mediator) : ControllerBase
    {
        //[HttpPost("student/register")]
        //public async Task<IActionResult> Register(RegisterStudentCommand command)
        //{
        //    command.AccessToken = accessToken;
        //    return Ok(await mediator.Send(command));
        //}

        [HttpPost("parent/register")]
        public async Task<IActionResult> RegisterParent(RegisterParentCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpPost("tutor/register")]
        public async Task<IActionResult> RegisterTutor(RegisterTutorCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpPost("tutor/additionaldetails")]
        public async Task<IActionResult> AdditionalDetails([FromForm] AddAdditionalTutorDetailsCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpGet("parent/all")]
        public async Task<IActionResult> Get([FromQuery]GetParentQuery query)
        {
            return Ok(await mediator.Send(query));
        }

        //[HttpPost("admin/register")]
        //public async Task<IActionResult> Register(RegisterAdminCommand command)
        //{
        //    return Ok(await _mediator.Send(command));
        //}

        [HttpPost("confirmregistration")]
        public async Task<IActionResult> ConfirmRegistration(VerifyEmailCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpPost("resendverificationcode")]
        public async Task<IActionResult> ResendCode(ResendVerificationCodeCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {

            var res = await mediator.Send(command);
            var token = (TokenResponse)res?.Entity;
            if (token != null)
            {
                HttpContext.Response.Cookies.Append("stem-prep-accessToken", token.AccessToken);
                HttpContext.Response.Cookies.Append("stem-prep-refreshToken", token.RefreshToken);
            }

            return Ok(res);
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgetPassword(ForgotPasswordCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpPost("verifyresetcode")]
        public async Task<IActionResult> VerifyResetCode(VerifyForgotPasswordCodeCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            return Ok(await mediator.Send(command));
        }
    }
}


