using Application.Interfaces;
using Application.Students.Commands;
using Application.Tutors.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TutorController(IMediator mediator, ITokenGenerator tokenGenerator) : ControllerBase
    {
        [Authorize]
        [HttpGet("dashboardinfo")]
        public async Task<IActionResult> GetInfo([FromQuery] GetTutorInfoCommand command)
        {
            Guid.TryParse(tokenGenerator.GetOwnerIdFromToken(User), out Guid TutorGuid);
            command.TutorGuid = TutorGuid;
            return Ok(await mediator.Send(command));
        }

    }
}
