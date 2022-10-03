using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace RecoverUnsoldDomain.Extensions;

public static class FormFileExtensions
{
    public static async Task<ImageUploadResult?> UploadToCloudinary(this IFormFile iFormFile, Cloudinary cloudinary)
    {
        await using var fileStream = iFormFile.OpenReadStream();
        var fileName = $"{DateTime.Now.Ticks}{Path.GetExtension(iFormFile.FileName)}";
        var imageUploadParameters = new ImageUploadParams
        {
            File = new FileDescription(fileName, fileStream)
        };
        return await cloudinary.UploadAsync(imageUploadParameters);
    }
}