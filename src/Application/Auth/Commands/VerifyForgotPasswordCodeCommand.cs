using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Application.Auth.Commands;

public class VerifyForgotPasswordCodeCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string PasswordResetCode { get; set; }
}

public class VerifyForgotPasswordCodeCommandHandler(IApplicationDbContext context, IConnectionMultiplexer redis) : IRequestHandler<VerifyForgotPasswordCodeCommand, Result>
{
    private readonly IDatabase _redisDb = redis.GetDatabase();

    public async Task<Result> Handle(VerifyForgotPasswordCodeCommand request, CancellationToken cancellationToken)
    {
        Student user = await context.Students.Where(s=> s.Email == request.Email).FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return Result.Failure<VerifyForgotPasswordCodeCommand>("Invalid email.");
        }

        string storedResetCode = await _redisDb.StringGetAsync($"PasswordResetCode:{request.Email}");
        if (string.IsNullOrEmpty(storedResetCode) || storedResetCode != request.PasswordResetCode)
        {
            return Result.Failure<VerifyForgotPasswordCodeCommand>("Invalid or expired password reset code.");
        }

        return Result.Success("Password reset code verified successfully.");
    }
}
