using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Dto;

namespace Application.Documents.Queries
{
    public class GetDocumentsByUserCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
    }

    public class GetDocumentsByUserCommandHandler(
        IApplicationDbContext context) : IRequestHandler<GetDocumentsByUserCommand, Result>
    {
        public async Task<Result> Handle(GetDocumentsByUserCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
            {
                return Result.Failure("Invalid user id");
            }
            var documents = await context.Documents
                            .Select(d => new DocumentDto
                            {
                                Guid = d.Guid,
                                CloudinaryUrl = d.CloudinaryUrl,
                                FileName = d.FileName,
                                FileType = d.FileType,
                                UserGuid = d.UserGuid,
                            })
                            .ToListAsync(cancellationToken);
            return Result.Success<GetDocumentsByUserCommand>("documents retrieved successfully", documents);
        }
    }



}
