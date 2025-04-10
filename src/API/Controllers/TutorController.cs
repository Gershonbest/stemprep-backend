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
        public async Task<IActionResult> GetInfo()
        {
            if (Guid.TryParse(tokenGenerator.GetOwnerIdFromToken(User), out Guid TutorGuid))
            {
                var command = new GetTutorInfoCommand { TutorGuid = TutorGuid };
                return Ok(await mediator.Send(command));
            }
            return BadRequest("Invalid User ID");
        }

        [Authorize(Roles = "Tutor")]
        [HttpGet("update")]
        public async Task<IActionResult> Update()
        {
            if (Guid.TryParse(tokenGenerator.GetOwnerIdFromToken(User), out Guid TutorGuid))
            {
                var command = new UpdateTutorCommand { TutorGuid = TutorGuid };
                return Ok(await mediator.Send(command));
            }
            return BadRequest("Invalid User ID");
        }
    }
}
