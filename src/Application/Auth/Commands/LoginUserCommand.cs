using Application.Common;
using Application.Common.Helpers;
using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using StackExchange.Redis;
using Microsoft.AspNetCore.Http;
using Domain.Common.Entities;

namespace Application.Auth.Commands;

public class LoginUserCommand<TUser> : IRequest<Result>
    where TUser : BaseUser
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginUserCommandHandler<TUser>(IApplicationDbContext context,
                                  ITokenGenerator generateToken,
                                  IEmailService emailService,
                                  IConnectionMultiplexer redis,
                                  IHttpContextAccessor httpContextAccessor,
                                  ISecretHasherService secretHasherService,
                                  IRefreshTokenService refreshTokenService) : IRequestHandler<LoginUserCommand<TUser>, Result>
    where TUser : BaseUser
{
    private readonly IDatabase _redisDb = redis.GetDatabase();

    public async Task<Result> Handle(LoginUserCommand<TUser> request, CancellationToken cancellationToken)
    {
        TUser user = await new AuthHelper(context).GetUserByEmail<TUser>(request.Email);

        if (user == null)
        {
            return Result.Failure<LoginUserCommand<TUser>>("Invalid Email or Password");
        }

        var userType = await new AuthHelper(context).GetUserTypeByEmail(request.Email);

        if (userType != typeof(TUser))
        {
            return Result.Failure<LoginUserCommand<TUser>>($"User is registered as a {userType.Name}");
        }

        if (!user.IsVerified)
        {
            // Generate and send a new confirmation code
            string confirmationCode = GenerateCode.GenerateRegistrationCode();

            // Store the confirmation code in Redis with an expiration of 2 hours
            await _redisDb.StringSetAsync($"RegistrationCode:{request.Email}", confirmationCode, TimeSpan.FromHours(2));

            // Send the confirmation code to the user's email
            await emailService.SendAccountConfirmationCodeAsync(user.Email!, confirmationCode);

            return Result.Failure<LoginUserCommand<TUser>>($"User {request.Email} account is not verified. A new confirmation code has been sent.");
        }

        string hashedPassword = secretHasherService.Hash(request.Password);
        if (user.PasswordHash != hashedPassword)
        {
            return Result.Failure<LoginUserCommand<TUser>>("Invalid Email or Password");
        }

        var tokens = generateToken.GenerateTokens(user.FirstName, user.Email!, user.UserType.ToString(), user.Guid);

        CookieHelper.SetTokensInCookies(httpContextAccessor, tokens.AccessToken, tokens.RefreshToken);

        await refreshTokenService.AddRefreshTokenAsync<TUser>(new RefreshToken
        {
            Token = tokens.RefreshToken,
            Expires = DateTime.Now.AddDays(30)
        });

        return Result.Success<LoginUserCommand<TUser>>("Successfully logged in", tokens);
    }
}
