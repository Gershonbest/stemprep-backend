using Application.Common.Models;
using Application.Extensions;
using Application.Interfaces;
using Domain.Common.Entities;
using MediatR;
using StackExchange.Redis;

namespace Application.Auth.Commands
{
    public class ResetPasswordCommand : IRequest<Result>, IPasswordValidator
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string PasswordResetToken { get; set; }
    }

    public class ResetPasswordCommandHandler(IApplicationDbContext context, IMediator mediator, IConnectionMultiplexer redis, ISecretHasherService secretHasherService) : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly IDatabase _redisDb = redis.GetDatabase();

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Validate password using custom validator
            await request.ValidateAsync(new PasswordValidator(), cancellationToken);

            BaseUser user = await new AuthHelper(context).GetBaseUserByEmail(request.Email);
            if (user == null)
            {
                return Result.Failure<ResetPasswordCommand>("Invalid email.");
            }

            string storedResetToken = await _redisDb.StringGetAsync($"PasswordResetToken:{request.Email}");
            if (string.IsNullOrEmpty(storedResetToken) || storedResetToken != request.PasswordResetToken)
            {
                return Result.Failure<VerifyForgotPasswordCodeCommand>("Invalid or expired password reset token.");
            }

            user.PasswordHash = secretHasherService.Hash(request.NewPassword);
            await context.SaveChangesAsync(cancellationToken);
            // Success response
            return Result.Success<ResetPasswordCommand>("Password reset successfully.");
        }
    }
}

