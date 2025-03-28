using Application.Auth.Commands;
using Application.Interfaces;
using Application.Students.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StudentController(IMediator mediator, ITokenGenerator tokenGenerator) : ControllerBase
    {

        [HttpPost("login")]
        public async Task<IActionResult> TutorLogin(LoginStudentCommand command)
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
        public async Task<IActionResult> ForgotPassword([FromBody]StudentForgotPasswordCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody]StudentResetPasswordCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [Authorize(Roles = "Parent")]
        [HttpPost("register")]
        public async Task<IActionResult> AddStudent([FromBody] RegisterStudentCommand command)
        {
            Guid.TryParse(tokenGenerator.GetOwnerIdFromToken(User), out Guid parentGuid);
            command.ParentGuid = parentGuid;
            return Ok(await mediator.Send(command));
        }

    }
}
