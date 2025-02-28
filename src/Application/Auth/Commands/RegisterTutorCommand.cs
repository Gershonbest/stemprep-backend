using Application.Common.Helpers;
using Application.Common.Models;
using Application.Extensions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using StackExchange.Redis;

namespace Application.Auth.Commands;

public class RegisterTutorCommand : IRequest<Result>, IUserValidator
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
   
}

public class RegisterTutorCommandHandler(
    IEmailService emailService,
    IConnectionMultiplexer redis,
    IApplicationDbContext context,
    ISecretHasherService secretHasherService) : IRequestHandler<RegisterTutorCommand, Result>
{
    private readonly IDatabase _redisDb = redis.GetDatabase();

    public async Task<Result> Handle(RegisterTutorCommand request, CancellationToken cancellationToken)
    {
        #region Validate inputs  
        await request.ValidateAsync(new UserCreateValidator(), cancellationToken);
        #endregion

        bool userExists = await new AuthHelper(context).CheckIfUserExists(request.Email);
        if (userExists)
        {
            return Result.Failure("User already exists. Please login or use a different email address.");
        }

        Tutor tutor = new()
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserType = UserType.Tutor,
            UserTypeDesc = UserType.Tutor.ToString(),
            IsVerified = true,
            AccountStatus = TutorAccountStatus.Pending,
            AccountStatusDesc = TutorAccountStatus.Pending.ToString(),
            AvailabilityStatus = AvailabilityStatus.Available,
            AvailabilityStatusDesc = AvailabilityStatus.Available.ToString(),
            UserStatus = Status.Active,
            UserStatusDes = Status.Active.ToString(),
            LastModifiedDate = DateTime.UtcNow,
            PasswordHash = secretHasherService.Hash(request.Password),
        };

        context.Tutors.Add(tutor);
        
        // Generate and save registration code to Redis
        string registrationCode = GenerateCode.GenerateRegistrationCode();
        await _redisDb.StringSetAsync($"RegistrationCode:{tutor.Email}", registrationCode, TimeSpan.FromHours(2));

        // Send the registration code to the user's email
        await emailService.SendRegistrationConfirmationEmailAsync(tutor.Email, tutor.FirstName, registrationCode);

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<RegisterTutorCommand>("Instructor registered successfully!");
    }
}
