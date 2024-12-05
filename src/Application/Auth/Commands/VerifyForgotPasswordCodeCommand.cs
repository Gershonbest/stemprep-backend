using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace Application.Auth.Commands;

public class VerifyForgotPasswordCodeCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string PasswordResetCode { get; set; }
}

public class VerifyForgotPasswordCodeCommandHandler : IRequestHandler<VerifyForgotPasswordCodeCommand, Result>
{
    private readonly UserManager<User> _userManager;
    private readonly IDatabase _redisDb;

    public VerifyForgotPasswordCodeCommandHandler(UserManager<User> userManager, IConnectionMultiplexer redis)
    {
        _userManager = userManager;
        _redisDb = redis.GetDatabase();
    }

    public async Task<Result> Handle(VerifyForgotPasswordCodeCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userManager.FindByEmailAsync(request.Email);
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
