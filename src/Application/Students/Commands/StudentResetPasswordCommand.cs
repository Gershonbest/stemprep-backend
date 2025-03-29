using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Application.Students.Commands
{
    public class StudentResetPasswordCommand : IRequest<Result>
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
    public class StudentResetPasswordCommandHandler(IApplicationDbContext context, IConnectionMultiplexer redis, ISecretHasherService secretHasherService, IConfiguration configuration)
    : IRequestHandler<StudentResetPasswordCommand, Result>
    {
        private readonly IDatabase _redisDb = redis.GetDatabase();

        public async Task<Result> Handle(StudentResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Find the student and update the password
            var student = await context.Students.FirstOrDefaultAsync(c => c.Username == request.Username, cancellationToken);
            if (student == null)
            {
                return Result.Failure("User not found");
            }

            // Retrieve the stored token from Redis
            string redisKey = configuration["Redis:StudentResetPassword"];
            var storedToken = await _redisDb.StringGetAsync(redisKey + request.Username);
            if (storedToken.IsNullOrEmpty || storedToken.ToString() != request.Token)
            {
                return Result.Failure("Invalid or expired reset token");
            }

            student.PasswordHash = secretHasherService.Hash(request.NewPassword);
            await context.SaveChangesAsync(cancellationToken);

            // Remove the token from Redis after successful reset
            await _redisDb.KeyDeleteAsync(redisKey + request.Username);

            return Result.Success("Password reset successfully");
        }
    }
}
