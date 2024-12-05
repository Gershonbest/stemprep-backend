using Application.Common.Helpers;
using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace Application.Auth.Commands
{
    public class ForgotPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; }
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IDatabase _redisDb;

        public ForgotPasswordCommandHandler(UserManager<User> userManager, IEmailService emailService, IConnectionMultiplexer redis)
        {
            _userManager = userManager;
            _emailService = emailService;
            _redisDb = redis.GetDatabase();
        }

        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            // Check if the user exists based on the provided email
            User? user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure("Email does not exist.");
            }

            // Use the PasswordReset helper to send the password reset verification code
            string verificationCode = await PasswordReset.SendPasswordResetVerificationCodeAsync(_redisDb.Multiplexer, _emailService, request.Email);

            // Return success result with the verification code
            return Result.Success("Email verification code sent via mail!");
        }
    }
}