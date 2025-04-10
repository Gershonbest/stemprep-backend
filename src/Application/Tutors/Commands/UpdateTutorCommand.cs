using Application.Common.Models;
using Application.Dto;
using Application.Interfaces;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tutors.Commands
{
    public class UpdateTutorCommand : IRequest<Result>
    {
        public Guid TutorGuid { get; set; }
        public TutorDto TutorDto { get; set; }
    }

    public class UpdateTutorStatusCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateTutorCommand, Result>
    {
        public async Task<Result> Handle(UpdateTutorCommand request, CancellationToken cancellationToken)
        {
            if (request.TutorGuid == Guid.Empty)
                return Result.Failure("Invalid Tutor Guid");
            if (request.TutorDto == null)
                return Result.Failure("Invalid Tutor Data");

            var tutor = await context.Tutors
                .FirstOrDefaultAsync(t => t.Guid == request.TutorGuid, cancellationToken);

            if (tutor == null)
                return Result.Failure("Tutor not found");

            if (request.TutorDto.FirstName != null)
                tutor.FirstName = request.TutorDto.FirstName;
            if (request.TutorDto.LastName != null)
                tutor.LastName = request.TutorDto.LastName;
            if (request.TutorDto.Email != null)
                tutor.Email = request.TutorDto.Email;
            if (request.TutorDto.AccountStatus > 0)
            {
                tutor.AccountStatus = request.TutorDto.AccountStatus;
                tutor.AccountStatusDesc = request.TutorDto.AccountStatus.ToString();
            }
            if (request.TutorDto.AvailabilityStatus > 0)
            {
                tutor.AvailabilityStatus = request.TutorDto.AvailabilityStatus;
                tutor.AvailabilityStatusDesc = request.TutorDto.AvailabilityStatus.ToString();
            }
            tutor.LastModifiedDate = DateTime.UtcNow;
            

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success("Tutor status updated successfully");
        }
    }
}
