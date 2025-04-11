using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Serilog;
using Domain.Entities;
using Application.Auth;
using Domain.Enum;

namespace Application.Documents.Commands
{
    public class UploadImageCommand : IRequest<Result>
    {
        public IFormFile ModuleImage { get; set; }
        public Guid UserGuid { get; set; }
    }

    public class UploadImageCommandHandler(IApplicationDbContext context, ICloudinaryService cloudinaryService) : IRequestHandler<UploadImageCommand, Result>
    {
        public async Task<Result> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            var user = await new AuthHelper(context).GetUserByGuid(request.UserGuid);
            if (user == null)
            {
                return Result.Failure("User does not exist");
            }

            // Upload the image to Cloudinary
            var result = await cloudinaryService.UploadImageAsync(request.ModuleImage);
            Document image;
            if (result.Succeeded)
            {
                Log.Information($"Document {request.ModuleImage.FileName} uploaded successfully to Cloudinary. URL: {result.Entity}");
                // Create and save Document entity
                image = new Document()
                {
                    Guid = Guid.NewGuid(),
                    CloudinaryUrl = result.Entity as string,
                    FileName = request.ModuleImage.FileName,
                    FileType = Path.GetExtension(request.ModuleImage.FileName).ToLowerInvariant(),
                    UserGuid = request.UserGuid,
                    UserType = user.UserType,
                    UserTypeDesc = user.UserType.ToString(),
                    DocumentType = DocumentType.Image,
                    DocumentTypeDesc = DocumentType.Image.ToString()
                };
                context.Documents.Add(image);
            }
            else
            {
                return Result.Failure($"Document upload failed for {request.ModuleImage.FileName}. {result.Error}");
            }

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success<UploadImageCommand>("image uploaded successfully!", image);
        }
    }

}
