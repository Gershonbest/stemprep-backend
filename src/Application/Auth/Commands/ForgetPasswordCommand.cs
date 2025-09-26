using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using StackExchange.Redis;

namespace Application.Auth.Commands
{
    public class ForgotPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; }
    }

    public class ForgotPasswordCommandHandler(IApplicationDbContext context, IEmailService emailService, IConnectionMultiplexer redis) : IRequestHandler<ForgotPasswordCommand, Result>
    {
        private readonly IDatabase _redisDb = redis.GetDatabase();

        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            // Check if the user exists based on the provided email
            bool userExists = await new AuthHelper(context).GlobalCheckIfUserExists(request.Email);
            if (!userExists)
            {
                return Result.Failure("Email does not exist.");
            }

            // Use the PasswordReset helper to send the password reset verification code
            string verificationCode = await PasswordReset.SendPasswordResetVerificationCodeAsync(_redisDb.Multiplexer, emailService, request.Email);

            // Return success result with the verification code
            return Result.Success("Email verification code sent via mail!");
        }
    }
}