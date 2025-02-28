using Application.Auth;
using Application.Common.Models;
using Application.Extensions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Application.Students.Commands;

public class RegisterStudentCommand : IRequest<Result>, IChildValidator
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    [JsonIgnore]
    public Guid ParentGuid { get; set; }
    public string Password { get; set; }
    public string Gender { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
}

public class RegisterStudentCommandHandler(
    IApplicationDbContext context,
    ISecretHasherService secretHasherService) : IRequestHandler<RegisterStudentCommand, Result>
{

    public async Task<Result> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
    {
        // Validate the request
        await request.ValidateAsync(new ChildCreateValidator(), cancellationToken);

        // Check if the student already exists
        Parent parent = await context.Parents
            .Include(x => x.Children)
            .Where(x => x.Guid == request.ParentGuid).FirstOrDefaultAsync(cancellationToken);

        if (parent is null)
            return Result.Failure("Parent not found");

        var childExists = await new AuthHelper(context).CheckIfChildExists(request.Username);
        if (childExists)
            return Result.Failure($"{request.Username} already exists");

        // Create the student entity
        Student student = new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.Username,
            PasswordHash = secretHasherService.Hash(request.Password),
            ParentEmail = parent.Email,
            UserType = UserType.Student,
            UserTypeDesc = UserType.Student.ToString(),
            IsVerified = false,
            UserStatus = Status.Active,
            UserStatusDes = Status.Active.ToString(),
            LastModifiedDate = DateTime.UtcNow,
            Gender = request.Gender,
            State = request.State,
            Country = request.Country
        };

        parent.Children.Add(student);

        context.Parents.Update(parent);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<RegisterStudentCommand>("Student added successfully");
    }
}
