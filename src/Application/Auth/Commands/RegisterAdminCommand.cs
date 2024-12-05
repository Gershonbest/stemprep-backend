using Application.Common.Helpers;
using Application.Common.Models;
using Application.Extensions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands;

public class RegisterAdminCommand : IRequest<Result>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

public class RegisterAdminCommandHandler : IRequestHandler<RegisterAdminCommand, Result>
{
    private readonly IEmailService _emailSender;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IDatabase _redisDb;
    private readonly IApplicationDbContext _context; 

    public RegisterAdminCommandHandler(
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

    public async Task<Result> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
    {
        // Check if the admin already exists
        User? userExist = await _userManager.FindByEmailAsync(request.Email);
        if (userExist != null)
            return Result.Failure(request, $"{userExist.Email} already exists");

        // Generate a random password for the admin
        string randomPassword = PasswordGenerator.GenerateRandomPassword();

        // Create the admin entity
        User user = new()
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserType = UserType.Admin,
            UserTypeDesc = UserType.Admin.ToString(),
            IsVerified = true,
            UserStatus = Status.Active,
            UserStatusDes = Status.Active.ToString(),
            SecurityStamp = Guid.NewGuid().ToString(),
            LastModifiedDate = DateTime.UtcNow
        };

        // Create the admin in the UserManager
        IdentityResult result = await _userManager.CreateAsync(user, randomPassword);
        if (!result.Succeeded)
        {
            string errors = string.Join("\n", result.Errors.Select(e => e.Description));
            return Result.Failure($"{user.UserTypeDesc} creation failed!\n" + errors);
        }

        // Check if role exists, and add admin to role
        string roleName = user.UserTypeDesc;
        if (!await _roleManager.RoleExistsAsync(roleName))
            await _roleManager.CreateAsync(new IdentityRole(roleName));

        if (await _roleManager.RoleExistsAsync(roleName))
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        // Send the email with admin login details (email and password)
        await _emailSender.SendLoginDetailsAsync(user.Email, "Admin", randomPassword);

        return Result.Success<RegisterAdminCommand>("Admin registered successfully, login details sent via email!", user.Id);
    }

  
}
