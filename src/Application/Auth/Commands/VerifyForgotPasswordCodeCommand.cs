using Application.Common.Models;
using Application.Interfaces;
using Domain.Common.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Security.Cryptography;
using System.Threading;

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
        BaseUser user = await context.Students.Where(s => s.Email == request.Email).FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return Result.Failure<VerifyForgotPasswordCodeCommand>("Invalid email.");
        }

        string storedResetCode = await _redisDb.StringGetAsync($"PasswordResetCode:{request.Email}");
        if (string.IsNullOrEmpty(storedResetCode) || storedResetCode != request.PasswordResetCode)
        {
            return Result.Failure<VerifyForgotPasswordCodeCommand>("Invalid or expired password reset code.");
        }

        string resetToken = GenerateToken();
        await _redisDb.StringSetAsync($"PasswordResetToken:{request.Email}", resetToken, TimeSpan.FromHours(1));


        return Result.Success("Password reset token generated successfully.", resetToken);

    }
    private static string GenerateToken()
    {
        byte[] tokenData = new byte[32];
        RandomNumberGenerator.Fill(tokenData);
        return Convert.ToBase64String(tokenData);
    }
}
