using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;


namespace Application.Auth.Commands;

public class ConfirmRegistrationCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string RegistrationCode { get; set; }
}

public class ConfirmRegistrationCommandHandler(
    IApplicationDbContext context,
    IConnectionMultiplexer redis)
    : IRequestHandler<ConfirmRegistrationCommand, Result>
{
    private readonly IDatabase _redisDb = redis.GetDatabase();

    public async Task<Result> Handle(ConfirmRegistrationCommand request, CancellationToken cancellationToken)
    {
        Student student = await context.Students.FirstOrDefaultAsync(s => s.Email == request.Email, cancellationToken);
        Parent parent = null; // Declare parent outside the if block

        if (student == null)
        {
            parent = await context.Parents.FirstOrDefaultAsync(t => t.Email == request.Email, cancellationToken);
            if (parent == null)
            {
                return Result.Failure("User not found.");
            }
        }

        string storedCode = await _redisDb.StringGetAsync($"RegistrationCode:{request.Email}");
        if (storedCode == null || storedCode != request.RegistrationCode)
            return Result.Failure("Invalid or expired registration code.");

        if (student != null) // Check if student is not null
        {
            student.IsVerified = true;
            student.UserStatus = Status.Active;
            student.UserStatusDes = Status.Active.ToString();

            context.Students.Update(student);
        }
        else if (parent != null) // Now handle the parent update
        {
            parent.IsVerified = true;
            parent.UserStatus = Status.Active;
            parent.UserStatusDes = Status.Active.ToString();

            context.Parents.Update(parent);
        }


        await context.SaveChangesAsync(cancellationToken);

        return Result.Success("Registration confirmed successfully!");
    }
}
