using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using NutriTrack.Tests;

namespace NutriTrack.Services;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public ImageService(IConfiguration configuration)
    {
        var account = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]
        );

        _cloudinary = new Cloudinary(account);
    }
    
    public async Task<string> UploadImageAsync(IFormFile file)
    {
        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Width(600).Height(600).Crop("limit")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
            throw new Exception(uploadResult.Error.Message);

        return uploadResult.SecureUrl.ToString();
    }
    
    public async Task<DeletionResult> DeleteImageAsync(string imageUrl)
    {
        var publicId = ExtractPublicId(imageUrl);

        Console.WriteLine($"Deleting image: {publicId}");
        
        var deleteParams = new DeletionParams(publicId);

        var result = await _cloudinary.DestroyAsync(deleteParams);

        return result;
    }
    
    private static string ExtractPublicId(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return string.Empty;

        var uri = new Uri(url);
        var segments = uri.Segments;
        if (segments.Length == 0) return string.Empty;

        var fileName = segments.Last().Trim('/');
        
        var lastDotIndex = fileName.LastIndexOf('.');
        if (lastDotIndex > 0)
        {
            fileName = fileName.Substring(0, lastDotIndex);
        }

        return fileName;
    }
}