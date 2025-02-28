using Application.Common;
using Application.Common.Helpers;
using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using StackExchange.Redis;
using Microsoft.AspNetCore.Http;
using Domain.Common.Entities;

namespace Application.Auth.Commands;

public class LoginUserCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class StudentLoginCommandHandler(IApplicationDbContext context,
                                  ITokenGenerator generateToken,
                                  IEmailService emailService,
                                  IConnectionMultiplexer redis,
                                  IHttpContextAccessor httpContextAccessor,
                                  ISecretHasherService secretHasherService) : IRequestHandler<LoginUserCommand, Result>
{
    private readonly IDatabase _redisDb = redis.GetDatabase();

    public async Task<Result> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        BaseUser user = await new AuthHelper(context).GetUserByEmail(request.Email);

        if (user == null)
        {
            return Result.Failure<LoginUserCommand>("Invalid Email or Password");
        }

        if (!user.IsVerified)
        {
            // Generate and send a new confirmation code
            string confirmationCode = GenerateCode.GenerateRegistrationCode();

            // Store the confirmation code in Redis with an expiration of 2 hours
            await _redisDb.StringSetAsync($"RegistrationCode:{request.Email}", confirmationCode, TimeSpan.FromHours(2));

            // Send the confirmation code to the user's email
            await emailService.SendAccountConfirmationCodeAsync(user.Email!, confirmationCode);

            return Result.Failure<LoginUserCommand>($"User {request.Email} account is not verified. A new confirmation code has been sent.");
        }

        string hashedPassword = secretHasherService.Hash(request.Password);
        if (user.PasswordHash != hashedPassword)
        {
            return Result.Failure<LoginUserCommand>("Invalid Email or Password");
        }

        var tokens = generateToken.GenerateTokens(user.FirstName, user.Email!, user.UserType.ToString(), user.Guid);

        CookieHelper.SetTokensInCookies(httpContextAccessor, tokens.AccessToken, tokens.RefreshToken);

        return Result.Success<LoginUserCommand>("Successfully logged in", tokens);
    }
}
