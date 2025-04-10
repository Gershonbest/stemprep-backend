using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Dto;

namespace Application.Documents.Queries
{
    public class GetDocumentsRequest : IRequest<Result>
    {

    }

    public class GetDocumentsCommandHandler(
        IApplicationDbContext context) : IRequestHandler<GetDocumentsRequest, Result>
    {

        public async Task<Result> Handle(GetDocumentsRequest request, CancellationToken cancellationToken)
        {
            var documents = await context.Documents
                            .Select(d => new DocumentDto
                            {
                                Guid = d.Guid,
                                CloudinaryUrl = d.CloudinaryUrl,
                                FileName = d.FileName,
                                FileType = d.FileType,
                                UserId = d.UserGuid,
                            })
                            .ToListAsync(cancellationToken);
            return Result.Success<GetDocumentsRequest>("documents retrieved successfully", documents);
        }
    }



}
