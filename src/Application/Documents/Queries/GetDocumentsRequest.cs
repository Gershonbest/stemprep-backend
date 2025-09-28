using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Dto;
using AutoMapper;

namespace Application.Documents.Queries
{
    public class GetDocumentsRequest : IRequest<Result>
    {

    }

    public class GetDocumentsCommandHandler(
        IApplicationDbContext context,IMapper mapper) : IRequestHandler<GetDocumentsRequest, Result>
    {

        public async Task<Result> Handle(GetDocumentsRequest request, CancellationToken cancellationToken)
        {
            var documents = await context.Documents
                            .ToListAsync(cancellationToken);
            mapper.Map<List<DocumentDto>>(documents);
            return Result.Success<GetDocumentsRequest>("documents retrieved successfully", documents);
        }
    }



}
