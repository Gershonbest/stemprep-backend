using Application.Common.Helpers;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Common.Entities;
using MediatR;
using StackExchange.Redis;

namespace Application.Auth.Commands;

public class ResendVerificationCodeCommand : IRequest<Result>
{
    public string Email { get; set; }
}


public class ResendConfirmationCodeCommandHandler(
    IEmailService emailSender,
    IConnectionMultiplexer redis,
    IApplicationDbContext context) : IRequestHandler<ResendVerificationCodeCommand, Result>
{
    private readonly IDatabase _redisDb = redis.GetDatabase();

    public async Task<Result> Handle(ResendVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        BaseUser user = await new AuthHelper(context).GetUserByEmail(request.Email);

        if (user == null)
        {
            return Result.Failure("User not found");
        }

        if (user.IsVerified)
        {
            return Result.Failure("User already registered.");
        }

        await _redisDb.KeyDeleteAsync($"RegistrationCode:{request.Email}");

        // Generate and save registration code to Redis
        string registrationCode = GenerateCode.GenerateRegistrationCode();
        await _redisDb.StringSetAsync($"RegistrationCode:{request.Email}", registrationCode, TimeSpan.FromHours(2));

        // Send the registration code to the user's email
        await emailSender.SendRegistrationConfirmationEmailAsync(request.Email, user.FirstName,registrationCode);


        return Result.Success<ResendVerificationCodeCommand>("Registration code sent successfully! Please confirm your registration.");
    }
}
