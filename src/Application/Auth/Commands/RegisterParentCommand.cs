using Application.Common.Helpers;
using Application.Common.Models;
using Application.Extensions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Application.Auth.Commands;

public class RegisterParentCommand : IRequest<Result>, IUserValidator
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
}


public class RegisterParentCommandHandler(
    IEmailService emailSender,
    IConnectionMultiplexer redis,
    IApplicationDbContext context) : IRequestHandler<RegisterParentCommand, Result>
{
    private readonly IDatabase _redisDb = redis.GetDatabase();

    public async Task<Result> Handle(RegisterParentCommand request, CancellationToken cancellationToken)
    {
        #region Validate inputs  
        await request.ValidateAsync(new UserCreateValidator(), cancellationToken);
        #endregion

        // Check if the student already exists
        bool userExist = await context.Parents.AnyAsync(p => p.Email == request.Email, cancellationToken);

        if (userExist)
            return Result.Failure(request, $"{request.Email} already exists");

        // Create the parent entity
        Parent user = new()
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserType = UserType.Student,
            UserTypeDesc = UserType.Student.ToString(),
            IsVerified = false,
            UserStatus = Status.Inactive,
            UserStatusDes = Status.Inactive.ToString(),
            LastModifiedDate = DateTime.UtcNow
        };

        context.Parents.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        // Generate and save registration code to Redis
        string registrationCode = GenerateCode.GenerateRegistrationCode();
        await _redisDb.StringSetAsync($"RegistrationCode:{user.Email}", registrationCode, TimeSpan.FromHours(2));

        // Send the registration code to the user's email
        await emailSender.SendRegistrationConfirmationEmailAsync(user.Email, user.FirstName, registrationCode);


        return Result.Success<RegisterParentCommand>("Registration code sent successfully! Please confirm your registration.", user);
    }
}
