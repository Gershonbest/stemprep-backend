
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace Application.Auth.Commands;

public class RegisterInstructorCommand : IRequest<Result>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

public class RegisterInstructorCommandHandler : IRequestHandler<RegisterInstructorCommand, Result>
{
    private readonly IEmailService _emailSender;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IDatabase _redisDb;
    private readonly IApplicationDbContext _context;

    public RegisterInstructorCommandHandler(
        IEmailService emailSender,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IConnectionMultiplexer redis,
        IApplicationDbContext context)
    {
        _emailSender = emailSender;
        _userManager = userManager;
        _roleManager = roleManager;
        _redisDb = redis.GetDatabase();
        _context = context;
    }

    public async Task<Result> Handle(RegisterInstructorCommand request, CancellationToken cancellationToken)
    {
        // Check if the instructor already exists
        User? userExist = await _userManager.FindByEmailAsync(request.Email);
        if (userExist != null)
            return Result.Failure(request, $"{userExist.Email} already exists");

        // Generate a random password for the instructor
        string randomPassword = PasswordGenerator.GenerateRandomPassword();

        // Create the instructor entity
        User user = new()
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserType = UserType.Instructor,
            UserTypeDesc = UserType.Instructor.ToString(),
            IsVerified = true,
            UserStatus = Status.Active,
            UserStatusDes = Status.Active.ToString(),
            SecurityStamp = Guid.NewGuid().ToString(),
            LastModifiedDate = DateTime.UtcNow
        };

        // Create the instructor in the UserManager
        IdentityResult result = await _userManager.CreateAsync(user, randomPassword);
        if (!result.Succeeded)
        {
            string errors = string.Join("\n", result.Errors.Select(e => e.Description));
            return Result.Failure($"{user.UserTypeDesc} creation failed!\n" + errors);
        }

        // Check if role exists, and add instructor to role
        string roleName = user.UserTypeDesc;
        if (!await _roleManager.RoleExistsAsync(roleName))
            await _roleManager.CreateAsync(new IdentityRole(roleName));

        if (await _roleManager.RoleExistsAsync(roleName))
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        // Send the email with instructor login details (email and password)
        await _emailSender.SendLoginDetailsAsync(user.Email, "Instructor", randomPassword);

        return Result.Success<RegisterInstructorCommand>("Instructor registered successfully, login details sent via email!", user.Id);
    }
}
