using Application.Common;
using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Domain.Entities;

namespace Application.Auth.Commands;

public class LoginStudentCommand : IRequest<Result>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginStudentCommandHandler(IApplicationDbContext context,
                                  ITokenGenerator generateToken,
                                  IHttpContextAccessor httpContextAccessor,
                                  ISecretHasherService secretHasherService) : IRequestHandler<LoginStudentCommand, Result>
{
    public async Task<Result> Handle(LoginStudentCommand request, CancellationToken cancellationToken)
    {
        Student user = await new AuthHelper(context).GetStudentByUsername(request.UserName);

        if (user == null)
        {
            return Result.Failure<LoginStudentCommand>("Invalid username or Password");
        }

        string hashedPassword = secretHasherService.Hash(request.Password);
        if (user.PasswordHash != hashedPassword)
        {
            return Result.Failure<LoginStudentCommand>("Invalid username or Password");
        }

        var tokens = generateToken.GenerateTokens(user.FirstName, user.Email!, user.UserType.ToString(), user.Guid);

        CookieHelper.SetTokensInCookies(httpContextAccessor, tokens.AccessToken, tokens.RefreshToken);

        return Result.Success<LoginStudentCommand>("Successfully logged in", tokens);
    }
}
