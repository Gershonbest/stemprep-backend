//using Application.Common.Helpers;
//using Application.Common.Models;
//using Application.Extensions;
//using Application.Interfaces;
//using Domain.Entities;
//using Domain.Enum;
//using MediatR;
//using StackExchange.Redis;

//namespace Application.Auth.Commands;

//public class RegisterStudentCommand : IRequest<Result>
//{
//    public string FirstName { get; set; }
//    public string LastName { get; set; }
//    public string Email { get; set; }
//    public string Password { get; set; }
//}

//public class RegisterStudentCommandHandler(
//    IEmailService emailSender,
//    IConnectionMultiplexer redis,
//    IApplicationDbContext context) : IRequestHandler<RegisterStudentCommand, Result>
//{
//    private readonly IDatabase _redisDb = redis.GetDatabase();

//    public async Task<Result> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
//    {
//        // Validate the request
//        await request.ValidateAsync(new UserCreateValidator(), cancellationToken);

//        // Check if the student already exists
//        Student? userExist = await _userManager.FindByEmailAsync(request.Email);
//        if (userExist != null)
//            return Result.Failure(request, $"{userExist.Email} already exists");


//        // Create the student entity
//        Student user = new()
//        {
//            Email = request.Email,
//            FirstName = request.FirstName,
//            LastName = request.LastName,
//            UserType = UserType.Student,
//            UserTypeDesc = UserType.Student.ToString(),
//            IsVerified = false,
//            UserStatus = Status.Inactive,
//            UserStatusDes = Status.Inactive.ToString(),
//            LastModifiedDate = DateTime.UtcNow
//        };

//        // Create the student in the UserManager
//        IdentityResult result = await _userManager.CreateAsync(user, request.Password);
//        if (!result.Succeeded)
//        {
//            string errors = string.Join("\n", result.Errors.Select(e => e.Description));
//            return Result.Failure($"{user.UserTypeDesc} creation failed!\n" + errors);
//        }

//        // Check if role exists, and add student to role
//        string roleName = user.UserTypeDesc;
//        if (!await _roleManager.RoleExistsAsync(roleName))
//            await _roleManager.CreateAsync(new IdentityRole(roleName));

//        if (await _roleManager.RoleExistsAsync(roleName))
//        {
//            await _userManager.AddToRoleAsync(user, roleName);
//        }

//        // Generate and save registration code to Redis
//        string registrationCode = GenerateCode.GenerateRegistrationCode();
//        await _redisDb.StringSetAsync($"RegistrationCode:{user.Email}", registrationCode, TimeSpan.FromHours(2));

//        // Send the registration code to the user's email
//        await emailSender.SendRegistrationConfirmationEmailAsync(user.Email, user.FirstName, registrationCode);


//        return Result.Success<RegisterStudentCommand>("Registration code sent successfully! Please confirm your registration.", user);
//    }
//}
