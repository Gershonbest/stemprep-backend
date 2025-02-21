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

    public class GetDocumentsbyUserCommandHandler(
        IApplicationDbContext context) : IRequestHandler<GetDocumentsByUserCommand, Result>
    {

        public async Task<Result> Handle(GetDocumentsByUserCommand request, CancellationToken cancellationToken)
        {
            var documents = await context.Documents
                            .Select(d => new DocumentDto
                            {
                                DocumentId = d.Guid,
                                CloudinaryUrl = d.CloudinaryUrl,
                                FileName = d.FileName,
                                FileType = d.FileType,
                                UserId = d.UserGuId,
                            })
                            .ToListAsync(cancellationToken);
            return Result.Success<GetDocumentsByUserCommand>("documents retrieved successfully", documents);
        }
    }



}
