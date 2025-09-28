using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Dto;
using AutoMapper;

namespace Application.Documents.Queries
{
    public class GetDocumentsByUserCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
    }

    public class GetDocumentsByUserCommandHandler(
        IApplicationDbContext context,IMapper mapper) : IRequestHandler<GetDocumentsByUserCommand, Result>
    {
        public async Task<Result> Handle(GetDocumentsByUserCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
            {
                return Result.Failure("Invalid user id");
            }
            var documents = await context.Documents
                            .Where(x => x.UserGuid == request.UserId)
                            .ToListAsync(cancellationToken);
            mapper.Map<List<DocumentDto>>(documents);
            return Result.Success<GetDocumentsByUserCommand>("documents retrieved successfully", documents);
        }
    }



}
