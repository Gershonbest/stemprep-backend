using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Parents.Queries
{
    public class GetParentQuery : IRequest<Result>
    {
    }

    public class GetParentQueryHandler(
        IApplicationDbContext context) : IRequestHandler<GetParentQuery, Result>
    {

        public async Task<Result> Handle(GetParentQuery request, CancellationToken cancellationToken)
        {
            var parents =  await context.Parents.ToListAsync(cancellationToken);

            return Result.Success<GetParentQuery>("parents retrieved succrssfully", parents);
        }
    }

}
