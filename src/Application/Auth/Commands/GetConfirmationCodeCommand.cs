using Application.Common.Helpers;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Application.Auth.Commands;

public class GetConfirmationCodeCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}

public class GetConfirmationCodeCommandHandler(
    IEmailService emailService,
    IConnectionMultiplexer redis,
    IApplicationDbContext context,
    ICloudinaryService cloudinaryService) : IRequestHandler<GetConfirmationCodeCommand, Result>
{
    private readonly IDatabase _redisDb = redis.GetDatabase();

    public async Task<Result> Handle(GetConfirmationCodeCommand request, CancellationToken cancellationToken)
    {
        #region business logic

        Tutor tutor = await context.Tutors.Where(t => t.Email == request.Email).FirstOrDefaultAsync(CancellationToken.None);

        bool userExists = await new AuthHelper(context).GlobalCheckIfUserExists(request.Email);
        if (tutor == null)
        {
            return Result.Failure("Tutor does not exist");
        }

        tutor.Email = request.Email;
        tutor.UserType = UserType.Tutor;
        tutor.UserTypeDesc = UserType.Tutor.ToString();
        tutor.IsVerified = true;
        tutor.UserStatus = Status.Active;
        tutor.UserStatusDes = Status.Active.ToString();
        tutor.LastModifiedDate = DateTime.UtcNow;

        context.Tutors.Update(tutor);

        // Generate and save registration code to Redis
        string registrationCode = GenerateCode.GenerateRegistrationCode();
        await _redisDb.StringSetAsync($"ConfirmationCode:{tutor.Email}", registrationCode, TimeSpan.FromHours(2));

        // Send the registration code to the user's email
        await emailService.SendRegistrationConfirmationEmailAsync(tutor.Email, tutor.FirstName, registrationCode);
        #endregion

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<GetConfirmationCodeCommand>("Instructor registered successfully, login details sent via email!", tutor);
    }
}



// if (request.Documents != null && request.Documents.Count != 0)
//        {
//            foreach (var document in request.Documents)
//            {
//                string fileExtension = Path.GetExtension(document.FileName).ToLowerInvariant();
//Log.Information($"Uploading document: {document.FileName}");
//                var result = await cloudinaryService.UploadFileAsync(document);
//                if (result.Succeeded)
//                {
//                    Log.Information($"Document {document.FileName} uploaded successfully to Cloudinary. URL: {result.Entity}");
//                    // Create and save Document entity
//                    var documentEntity = new Document()
//                    {
//                        Guid = Guid.NewGuid(),
//                        CloudinaryUrl = result.Entity as string,
//                        FileName = document.FileName,
//                        FileType = Path.GetExtension(document.FileName).ToLowerInvariant(),
//                        UserGuId = tutor.Guid,
//                        UserType = UserType.Tutor,
//                        UserTypeDesc = UserType.Tutor.ToString()
//                    };
//context.Documents.Add(documentEntity);
//                }
//                else
//{
//    return result;
//}
//            }
//            Log.Information("Document uploads to Cloudinary completed.");
//        }