using Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface ICloudinaryService
    {
        Task<Result> UploadFileAsync(IFormFile file);
        Task<Result> UploadImageAsync(IFormFile file);
    }
}
