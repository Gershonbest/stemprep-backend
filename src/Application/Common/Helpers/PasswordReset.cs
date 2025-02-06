using Application.Common.Helpers;
using Application.Interfaces;
using StackExchange.Redis;
using System.Threading.Tasks;

public static class PasswordReset
{
    public static async Task<string> SendPasswordResetVerificationCodeAsync(
        IConnectionMultiplexer redis,
        IEmailService emailService,
        string email)
    {
        // Get the Redis database instance
        var redisDb = redis.GetDatabase();

        // Check if the verification code already exists in Redis
        string existingCode = await redisDb.StringGetAsync($"PasswordResetCode:{email}");

        // If a code already exists, use it
        if (!string.IsNullOrEmpty(existingCode))
        {
            // Send the existing verification code to the user's email
            await emailService.SendPasswordResetCodeAsync(email,
                existingCode);

            // Return the existing verification code
            return existingCode;
        }

        // Generate a new verification code if it doesn't exist
        string newVerificationCode = GenerateCode.GenerateRegistrationCode();

        // Store the new reset code in Redis with an expiration of 2 hours
        await redisDb.StringSetAsync($"PasswordResetCode:{email}", newVerificationCode, TimeSpan.FromHours(2));

        // Send the new verification code to the user's email
        await emailService.SendPasswordResetCodeAsync(email,
            $"Your verification code is {newVerificationCode}");

        // Return the generated verification code
        return newVerificationCode;
    }
}
