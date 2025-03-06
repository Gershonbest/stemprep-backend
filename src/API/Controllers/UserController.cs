using Application.Interfaces;
using Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController(IMediator mediator, ITokenGenerator tokenGenerator) : ControllerBase
    {
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetInfo([FromQuery] GetUserInfoCommand command)
        {
            Guid.TryParse(tokenGenerator.GetOwnerIdFromToken(User), out Guid UserGuid);
            command.UserGuid = UserGuid;
            return Ok(await mediator.Send(command));
        }

    }
}
