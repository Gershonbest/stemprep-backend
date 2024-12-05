using Application.Common.Models;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;


namespace Application.Auth.Commands;

public class ConfirmRegistrationCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string RegistrationCode { get; set; }
}

public class ConfirmRegistrationCommandHandler(
    UserManager<User> userManager,
    IConnectionMultiplexer redis)
    : IRequestHandler<ConfirmRegistrationCommand, Result>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IDatabase _redisDb = redis.GetDatabase();

    public async Task<Result> Handle(ConfirmRegistrationCommand request, CancellationToken cancellationToken)
    {
        User? student = await _userManager.FindByEmailAsync(request.Email);
        if (student == null)
            return Result.Failure("Student not found.");

        string? storedCode = await _redisDb.StringGetAsync($"RegistrationCode:{request.Email}");
        if (storedCode == null || storedCode != request.RegistrationCode)
            return Result.Failure("Invalid or expired registration code.");

        // Update student's status to active
        student.IsVerified = true;
        student.UserStatus = Status.Active;
        student.UserStatusDes = Status.Active.ToString();

        IdentityResult result = await _userManager.UpdateAsync(student);
        if (!result.Succeeded)
        {
            string errors = string.Join("\n", result.Errors.Select(e => e.Description));
            return Result.Failure("Failed to confirm registration!\n" + errors);
        }

        return Result.Success("Registration confirmed successfully!");
    }
}