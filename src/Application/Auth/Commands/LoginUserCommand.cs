using Application.Common;
using Application.Common.Helpers;
using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using StackExchange.Redis;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Application.Auth.Commands;

public class LoginUserCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class StudentLoginCommandHandler : IRequestHandler<LoginUserCommand, Result>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IEmailService _emailService;
    private readonly IDatabase _redisDb;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StudentLoginCommandHandler(SignInManager<User> signInManager,
                                      UserManager<User> userManager,
                                      ITokenGenerator generateToken,
                                      IEmailService emailService,
                                      IConnectionMultiplexer redis,
                                      IHttpContextAccessor httpContextAccessor)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenGenerator = generateToken;
        _emailService = emailService;
        _redisDb = redis.GetDatabase();
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return Result.Failure<LoginUserCommand>("User Not Found.");
        }

        if (!user.IsVerified)
        {
            // Generate and send a new confirmation code
            string confirmationCode = GenerateCode.GenerateRegistrationCode();

            // Store the confirmation code in Redis with an expiration of 2 hours
            await _redisDb.StringSetAsync($"ConfirmationCode:{request.Email}", confirmationCode, TimeSpan.FromHours(2));

            // Send the confirmation code to the user's email
            await _emailService.SendAccountConfirmationCodeAsync(user.Email!, confirmationCode);

            return Result.Failure<LoginUserCommand>($"User {request.Email} account is not verified. A new confirmation code has been sent.");
        }

        var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, isPersistent: false, lockoutOnFailure: true);

        if (!signInResult.Succeeded)
        {
            if (signInResult.IsLockedOut)
            {
                user.UserStatus = Status.Suspended;
                user.UserStatusDes = Status.Suspended.ToString();
                await _userManager.UpdateAsync(user);

                return Result.Failure<LoginUserCommand>($"User {request.Email} account locked: Unsuccessful 3 login attempts.");
            }

            return Result.Failure<LoginUserCommand>("Invalid Email or Password");
        }

        var tokens = _tokenGenerator.GenerateTokens(user.Id, user.Email!, user.UserType.ToString());

        CookieHelper.SetTokensInCookies(_httpContextAccessor, tokens.AccessToken, tokens.RefreshToken);
    
        return Result.Success<LoginUserCommand>("Successfully logged in", tokens);  
    }
}
