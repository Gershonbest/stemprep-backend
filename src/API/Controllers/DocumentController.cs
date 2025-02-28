using Application.Documents.Commands;
using Application.Documents.Queries;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DocumentController(IMediator mediator, ITokenGenerator tokenGenerator) : ControllerBase
    {
        [HttpGet("openall")]
        public async Task<IActionResult> GetDocuments([FromQuery]GetDocumentsRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [Authorize(Roles = "Tutor")]
        [HttpGet("all")]
        public async Task<IActionResult> GetDocumentsByUser()
        {
            Guid.TryParse(tokenGenerator.GetOwnerIdFromToken(User), out Guid tutorGuid);
            var query = new GetDocumentsByUserCommand
            {
                UserId = tutorGuid
            };
            return Ok(await mediator.Send(query));
        }

        [Authorize]
        [HttpPost("image")]
        public async Task<IActionResult> UploadImage([FromForm]UploadImageCommand request)
        {
            return Ok(await mediator.Send(request));
        }

        [Authorize(Roles = "Tutor")]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadDocumentCommand command)
        {
            Guid.TryParse(tokenGenerator.GetOwnerIdFromToken(User), out Guid tutorGuid);
            command.UserGuid = tutorGuid;
            return Ok(await mediator.Send(command));
        }

        [Authorize(Roles = "Tutor")]
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteDocumentCommand command)
        {
            Guid.TryParse(tokenGenerator.GetOwnerIdFromToken(User), out Guid tutorGuid);
            command.UserGuid = tutorGuid;
            return Ok(await mediator.Send(command));
        }
    }
}
