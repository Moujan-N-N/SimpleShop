using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _environment;

    public ImageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string?> SaveImageAsync(IFormFile? imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
            return null;

        string uploadsFolder = Path.Combine(
            _environment.WebRootPath,
            "images",
            "products");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        string fileName =
            Guid.NewGuid().ToString() +
            Path.GetExtension(imageFile.FileName);

        string filePath = Path.Combine(uploadsFolder, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);

        await imageFile.CopyToAsync(stream);

        return "/images/products/" + fileName;
    }
}