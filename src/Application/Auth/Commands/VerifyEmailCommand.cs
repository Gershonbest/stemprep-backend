using Application.Common.Models;
using Application.Interfaces;
using Domain.Common.Entities;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using StackExchange.Redis;


namespace Application.Auth.Commands;

public class VerifyEmailCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string RegistrationCode { get; set; }
}

public class ConfirmRegistrationCommandHandler(
    IApplicationDbContext context,
    IConnectionMultiplexer redis)
    : IRequestHandler<VerifyEmailCommand, Result>
{
    private readonly IDatabase _redisDb = redis.GetDatabase();

    public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        BaseUser user = await new AuthHelper(context).GetBaseUserByEmail(request.Email);

        if (user == null)
        {
            return Result.Failure("User not found.");
        }

        string storedCode = await _redisDb.StringGetAsync($"RegistrationCode:{request.Email}");
        if (storedCode == null || storedCode != request.RegistrationCode)
            return Result.Failure("Invalid or expired registration code.");

        user.IsVerified = true;
        user.UserStatus = Status.Active;
        user.UserStatusDes = Status.Active.ToString();

        if (user is Student student)
        {
            context.Students.Update(student);
        }
        else if (user is Parent parent)
        {
            context.Parents.Update(parent);
        }
        else if (user is Tutor tutor)
        {
            context.Tutors.Update(tutor);
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success("Registration confirmed successfully!");
    }
}
