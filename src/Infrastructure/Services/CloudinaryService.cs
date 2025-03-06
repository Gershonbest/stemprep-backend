using Application.Common.Models;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using MediatR;

namespace Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _configuration;

        public CloudinaryService(IConfiguration configuration)
        {
            _configuration = configuration;

            Account account = new Account(
                _configuration["Cloudinary:CloudName"],
                _configuration["Cloudinary:ApiKey"],
                _configuration["Cloudinary:ApiSecret"]
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<Result> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length <= 0)
                return Result.Failure("No file or empty file provided for upload.");

            if (file.Length > 20 * 1024 * 1024) // Example: 20MB limit - adjust as needed
                return Result.Failure("File size exceeds the allowed limit (20MB).");

            var allowedExtensions = _configuration.GetSection("AllowedFileExtensions").Get<string[]>();
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
                return Result.Failure("Invalid file type. Only PDF and DOC files are allowed.");

            try
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "tutor_documents",
                    PublicId = Guid.NewGuid().ToString(),
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    return Result.Failure($"Cloudinary error: {uploadResult.Error.Message}");
                }

                return Result.Success(uploadResult.SecureUrl.ToString());
            }
            catch (Exception ex)
            {
                return Result.Failure($"Exception during file upload to Cloudinary: {ex.Message}");
            }
        }

        public async Task<Result> UploadImageAsync(IFormFile moduleImage)
        {
            ImageUploadParams uploadParams = new()
            {
                File = new FileDescription(moduleImage.FileName, moduleImage.OpenReadStream()),
                Transformation = new Transformation().Crop("limit").Width(800).Height(600).Quality("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return Result.Failure("Image upload failed!");
            }

            return Result.Success(uploadResult.SecureUrl.ToString());
        }

        public async Task<Result> DeleteFileAsync(string secureUrl)
        {
            if (string.IsNullOrEmpty(secureUrl))
                return Result.Failure("ID must be provided for deletion.");

            string publicId = ExtractPublicId(secureUrl);

            try
            {
                var deletionParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deletionParams);

                if (result.Result == "ok")
                {
                    return Result.Success("File deleted successfully.");
                }

                return Result.Failure("Failed to delete the file from Cloudinary.");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Exception during file deletion: {ex.Message}");
            }
        }

        public async Task<Result> EditImageAsync(string secureUrl, IFormFile newImage)
        {
            if (string.IsNullOrEmpty(secureUrl) || newImage == null || newImage.Length == 0)
                return Result.Failure("Invalid input: Public ID and a valid image file are required.");

            string publicId = ExtractPublicId(secureUrl);

            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(newImage.FileName, newImage.OpenReadStream()),
                    PublicId = publicId, // Overwrites the existing image
                    Overwrite = true,
                    Transformation = new Transformation().Width(800).Height(600).Crop("limit").Quality("auto")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    return Result.Failure($"Cloudinary error: {uploadResult.Error.Message}");
                }

                return Result.Success(uploadResult.SecureUrl.ToString());
            }
            catch (Exception ex)
            {
                return Result.Failure($"Exception during image update: {ex.Message}");
            }
        }

        private static string ExtractPublicId(string secureUrl)
        {
            var uri = new Uri(secureUrl);
            var filename = Path.GetFileNameWithoutExtension(uri.AbsolutePath);
            return filename;
        }

    }
}