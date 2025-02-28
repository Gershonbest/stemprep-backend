using Application.Auth;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Application.Students.Commands
{
    public class StudentForgotPasswordCommand : IRequest<Result>
    {
        public string Username { get; set; }
    }

    public class StudentForgotPasswordCommandHandler(IApplicationDbContext context, IEmailService emailService, IConnectionMultiplexer redis, IConfiguration configuration) : IRequestHandler<StudentForgotPasswordCommand, Result>
    {
        private readonly IDatabase _redisDb = redis.GetDatabase();

        public async Task<Result> Handle(StudentForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            // Check if the student already exists
            var childExists = await new AuthHelper(context).CheckIfChildExists(request.Username);
            if (!childExists)
                return Result.Failure("Invalid username");

            Parent parent = await context.Parents
                .Include(x => x.Children)
                .Where(x => x.Children.Any(c => c.Username == request.Username))
                .FirstOrDefaultAsync(cancellationToken);

            if (parent == null || string.IsNullOrEmpty(parent.Email))
                return Result.Failure("Parent email not found");

            string emailToSendTo = parent.Email;

            // Generate a password reset token
            string resetToken = Guid.NewGuid().ToString();

            string baseUrl = configuration["Url:BaseUrl"];
            var resetLink = $"{baseUrl}/reset-password?token={resetToken}&username={request.Username}";

            string redisKey = configuration["Redis:StudentResetPassword"];
            await _redisDb.StringSetAsync(redisKey + request.Username, resetToken, TimeSpan.FromMinutes(15));

            //TODO:Beautify the email sent
            // Send email with the reset link
            var emailSubject = "Password Reset Request";
            var emailBody = $"Click the link to reset your password: {resetLink}";
            await emailService.SendEmailAsync(emailToSendTo, emailSubject, emailBody);

            // Return success result with the verification code
            return Result.Success("Email password reset link sent");
        }
    }
}
