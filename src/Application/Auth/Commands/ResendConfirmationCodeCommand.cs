using Application.Common.Helpers;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Application.Auth.Commands;

public class ResendConfirmationCodeCommand : IRequest<Result>
{
    public string Email { get; set; }
}


public class ResendConfirmationCodeCommandHandler(
    IEmailService emailSender,
    IConnectionMultiplexer redis,
    IApplicationDbContext context) : IRequestHandler<ResendConfirmationCodeCommand, Result>
{
    private readonly IDatabase _redisDb = redis.GetDatabase();

    public async Task<Result> Handle(ResendConfirmationCodeCommand request, CancellationToken cancellationToken)
    {
        Student student = await context.Students.FirstOrDefaultAsync(s => s.Email == request.Email, cancellationToken);
        Parent parent = null; 

        if (student == null)
        {
            parent = await context.Parents.FirstOrDefaultAsync(t => t.Email == request.Email, cancellationToken);
            if (parent == null)
            {
                return Result.Failure("User not found.");
            }
        }

        if (student?.IsVerified == true || parent?.IsVerified == true)
        {
            return Result.Failure("User already registered.");
        }

        bool deleted = await _redisDb.KeyDeleteAsync($"RegistrationCode:{request.Email}");

        // Generate and save registration code to Redis
        string registrationCode = GenerateCode.GenerateRegistrationCode();
        await _redisDb.StringSetAsync($"RegistrationCode:{request.Email}", registrationCode, TimeSpan.FromHours(2));

        // Send the registration code to the user's email
        await emailSender.SendRegistrationConfirmationEmailAsync(request.Email, student == null ? parent.FirstName : student.FirstName,registrationCode);


        return Result.Success<ResendConfirmationCodeCommand>("Registration code sent successfully! Please confirm your registration.");
    }
}
