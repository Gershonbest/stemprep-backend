using Application.Common.Helpers;
using Application.Common.Models;
using Application.Extensions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace Application.Auth.Commands;

public class RegisterStudentCommand : IRequest<Result>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterStudentCommandHandler : IRequestHandler<RegisterStudentCommand, Result>
{
    private readonly IEmailService _emailSender;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IDatabase _redisDb;
    private readonly IApplicationDbContext _context; // Add the context for database interaction

    public RegisterStudentCommandHandler(
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

    public async Task<Result> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
    {
        // Validate the request
        await request.ValidateAsync(new UserCreateValidator(), cancellationToken);

        // Check if the student already exists
        User? userExist = await _userManager.FindByEmailAsync(request.Email);
        if (userExist != null)
            return Result.Failure(request, $"{userExist.Email} already exists");


        // Create the student entity
        User user = new()
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserType = UserType.Student,
            UserTypeDesc = UserType.Student.ToString(),
            IsVerified = false,
            UserStatus = Status.Inactive,
            UserStatusDes = Status.Inactive.ToString(),
            SecurityStamp = Guid.NewGuid().ToString(),
            LastModifiedDate = DateTime.UtcNow
        };

        // Create the student in the UserManager
        IdentityResult result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            string errors = string.Join("\n", result.Errors.Select(e => e.Description));
            return Result.Failure($"{user.UserTypeDesc} creation failed!\n" + errors);
        }

        // Check if role exists, and add student to role
        string roleName = user.UserTypeDesc;
        if (!await _roleManager.RoleExistsAsync(roleName))
            await _roleManager.CreateAsync(new IdentityRole(roleName));

        if (await _roleManager.RoleExistsAsync(roleName))
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        // Generate and save registration code to Redis
        string registrationCode = GenerateCode.GenerateRegistrationCode();
        await _redisDb.StringSetAsync($"RegistrationCode:{user.Email}", registrationCode, TimeSpan.FromHours(2));

        // Send the registration code to the user's email
        await _emailSender.SendRegistrationConfirmationEmailAsync(user.Email, user.FirstName, registrationCode);


        return Result.Success<RegisterStudentCommand>("Registration code sent successfully! Please confirm your registration.", user.Id);
    }
}
