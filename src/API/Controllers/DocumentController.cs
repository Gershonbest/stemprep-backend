using Application.Documents.Commands;
using Application.Documents.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DocumentController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetDocuments([FromQuery]GetDocumentsRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetDocumentsByUser(Guid userId)
        {
            var query = new GetDocumentsByUserCommand
            {
                UserId = userId
            };
            return Ok(await mediator.Send(query));
        }

        [HttpPost("Image")]
        public async Task<IActionResult> UploadImage(UploadImageCommand request)
        {
            return Ok(await mediator.Send(request));
        }
    }
}
