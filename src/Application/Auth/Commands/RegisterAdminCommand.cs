//using Application.Common.Helpers;
//using Application.Common.Models;
//using Application.Extensions;
//using Application.Interfaces;
//using Domain.Entities;
//using Domain.Enum;
//using MediatR;
//using StackExchange.Redis;
//using Microsoft.EntityFrameworkCore;

//namespace Application.Auth.Commands;

//public class RegisterAdminCommand : IRequest<Result>
//{
//    public string FirstName { get; set; }
//    public string LastName { get; set; }
//    public string Email { get; set; }
//    public string Password { get; set; }
//}

//public class RegisterAdminCommandHandler : IRequestHandler<RegisterAdminCommand, Result>
//{
//    private readonly IEmailService _emailService;
//    private readonly UserManager<Student> _userManager;
//    private readonly RoleManager<IdentityRole> _roleManager;
//    private readonly IDatabase _redisDb;
//    private readonly IApplicationDbContext _context; 

//    public RegisterAdminCommandHandler(
//        IEmailService emailSender,
//        UserManager<Student> userManager,
//        RoleManager<IdentityRole> roleManager,
//        IConnectionMultiplexer redis,
//        IApplicationDbContext context) 
//    {
//        _emailService = emailSender;
//        _userManager = userManager;
//        _roleManager = roleManager;
//        _redisDb = redis.GetDatabase();
//        _context = context;
//    }

//    public async Task<Result> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
//    {
//        // Check if the admin already exists
//        Student? userExist = await _userManager.FindByEmailAsync(request.Email);
//        if (userExist != null)
//            return Result.Failure(request, $"{userExist.Email} already exists");


//        // Create the admin entity
//        Student user = new()
//        {
//            Email = request.Email,
//            FirstName = request.FirstName,
//            LastName = request.LastName,
//            UserType = UserType.Admin,
//            UserTypeDesc = UserType.Admin.ToString(),
//            IsVerified = true,
//            UserStatus = Status.Active,
//            UserStatusDes = Status.Active.ToString(),
//            LastModifiedDate = DateTime.UtcNow
//        };

//        // Create the admin in the UserManager
//        IdentityResult result = await _userManager.CreateAsync(user, request.Password);
//        if (!result.Succeeded)
//        {
//            string errors = string.Join("\n", result.Errors.Select(e => e.Description));
//            return Result.Failure($"{user.UserTypeDesc} creation failed!\n" + errors);
//        }

//        // Check if role exists, and add admin to role
//        string roleName = user.UserTypeDesc;
//        if (!await _roleManager.RoleExistsAsync(roleName))
//            await _roleManager.CreateAsync(new IdentityRole(roleName));

//        if (await _roleManager.RoleExistsAsync(roleName))
//        {
//            await _userManager.AddToRoleAsync(user, roleName);
//        }


//        return Result.Success<RegisterAdminCommand>("Admin registered successfully", user);
//    }

  
//}
