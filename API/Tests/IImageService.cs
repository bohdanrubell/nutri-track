using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace NutriTrack.Tests
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<DeletionResult> DeleteImageAsync(string imageUrl);
    }
} 