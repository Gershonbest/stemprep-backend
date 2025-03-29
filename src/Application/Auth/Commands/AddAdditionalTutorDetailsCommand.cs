using Application.Common.Helpers;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Application.Auth.Commands;

public class AddAdditionalTutorDetailsCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string Gender { get; set; }
    public string PhoneNumber { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string Province { get; set; }
}

public class AddAdditionalTutorDetailsCommandHandler(
    IConnectionMultiplexer redis,
    IApplicationDbContext context) : IRequestHandler<AddAdditionalTutorDetailsCommand, Result>
{
    private readonly IDatabase _redisDb = redis.GetDatabase();

    public async Task<Result> Handle(AddAdditionalTutorDetailsCommand request, CancellationToken cancellationToken)
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
        tutor.Gender = request.Gender;
        tutor.Country = request.Country;
        tutor.State = request.State;
        tutor.Province = request.Province;

        context.Tutors.Update(tutor);

        #endregion

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<AddAdditionalTutorDetailsCommand>("Instructor registered successfully", tutor);
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