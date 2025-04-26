using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace NutriTrack.Tests
{
    public class FakeImageService : IImageService
    {
        public Task<string> UploadImageAsync(IFormFile file)
        {
            return Task.FromResult("test-image-url");
        }

        public Task<DeletionResult> DeleteImageAsync(string imageUrl)
        {
            return Task.FromResult(new DeletionResult());
        }
    }
} 