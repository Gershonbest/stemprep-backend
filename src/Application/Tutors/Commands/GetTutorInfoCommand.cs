using Application.Common.Models;
using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tutors.Commands
{
    public class GetTutorInfoCommand : IRequest<Result>
    {
        public Guid TutorGuid { get; set; }
    }

    public class GetTutorInfoCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetTutorInfoCommand, Result>
    {
        public async Task<Result> Handle(GetTutorInfoCommand request, CancellationToken cancellationToken)
        {
            var tutor = await context.Tutors
                .Where(x => x.Guid == request.TutorGuid)
                .FirstOrDefaultAsync(cancellationToken);

            if (tutor == null)
                return Result.Failure("Tutor not found");

            var tutorDto = mapper.Map<TutorDto>(tutor);

            return Result.Success(tutorDto);
        }
    }
}
