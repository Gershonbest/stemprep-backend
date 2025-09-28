
using Application.Auth;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Web.Helpers;

namespace Application.Documents.Commands
{
    public class EditImageCommand : IRequest<Result>
    {
        public IFormFile ModuleImage { get; set; }
        public Guid ImageGuid { get; set; }
        public Guid UserGuid { get; set; }
    }

    public class EditImageCommandHandler(IApplicationDbContext context, ICloudinaryService cloudinaryService) : IRequestHandler<EditImageCommand, Result>
    {
        public async Task<Result> Handle(EditImageCommand request, CancellationToken cancellationToken)
        {
            var user = await new AuthHelper(context).GetUserByGuid(request.UserGuid);
            if (user == null)
            {
                return Result.Failure("User does not exist");
            }

            var oldimage = context.Documents.Where(x => x.Guid == request.ImageGuid && x.UserGuid == request.UserGuid).FirstOrDefault();

            if (oldimage == null)
            {
                return Result.Failure("Document not found");
            }

            var newImage = new Document()
            {
                Guid = Guid.NewGuid(),
                CloudinaryUrl = oldimage.CloudinaryUrl,
                FileName = request.ModuleImage.FileName,
                FileType = Path.GetExtension(request.ModuleImage.FileName).ToLowerInvariant(),
                UserGuid = request.UserGuid,
                UserType = user.UserType,
                UserTypeDesc = user.UserType.ToString(),
                DocumentType = oldimage.DocumentType,
                DocumentTypeDesc = oldimage.DocumentType.ToString()
            };

            context.Documents.Remove(oldimage);
            context.Documents.Add(newImage);
            await context.SaveChangesAsync(cancellationToken);

            var result = await cloudinaryService.EditImageAsync(oldimage.CloudinaryUrl, request.ModuleImage);


            return Result.Success<EditImageCommand>("image updated successfully!");
        }
    }
}
