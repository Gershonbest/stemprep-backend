﻿using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Serilog;
using Domain.Entities;
using Application.Auth;
using System.Text.Json.Serialization;

namespace Application.Documents.Commands
{
    public class UploadDocumentCommand : IRequest<Result>
    {
        public IFormFile Document { get; set; }
        [JsonIgnore]
        public Guid UserGuid { get; set; }
    }

    public class UploadDocumentCommandHandler(IApplicationDbContext context, ICloudinaryService cloudinaryService) : IRequestHandler<UploadDocumentCommand, Result>
    {
        public async Task<Result> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            var user = await new AuthHelper(context).GetUserByGuid(request.UserGuid);
            if (user == null)
            {
                return Result.Failure("User does not exist");
            }

            // Upload the image to Cloudinary
            var result = await cloudinaryService.UploadFileAsync(request.Document);
            if (result.Succeeded)
            {
                Log.Information($"Document {request.Document.FileName} uploaded successfully to Cloudinary. URL: {result.Entity}");
                // Create and save Document entity
                var image = new Document()
                {
                    CloudinaryUrl = result.Entity as string,
                    FileName = request.Document.FileName,
                    FileType = Path.GetExtension(request.Document.FileName).ToLowerInvariant(),
                    UserGuid = request.UserGuid,
                    UserType = user.UserType,
                    UserTypeDesc = user.UserType.ToString()
                };
                context.Documents.Add(image);
            }
            else
            {
                return Result.Failure($"Document upload failed for {request.Document.FileName}. {result.Error}");
            }

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success<UploadDocumentCommand>("image uploaded successfully!");
        }
    }

}

