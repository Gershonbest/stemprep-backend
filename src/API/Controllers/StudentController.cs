using Application.Interfaces;
using Application.Students.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StudentController(IMediator mediator, ITokenGenerator tokenGenerator) : ControllerBase
    {

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
        [HttpPost("add")]
        public async Task<IActionResult> AddStudent([FromBody] RegisterStudentCommand command)
        {
            Guid.TryParse(tokenGenerator.GetOwnerIdFromToken(User), out Guid parentGuid);
            command.ParentGuid = parentGuid;
            return Ok(await mediator.Send(command));
        }

    }
}
