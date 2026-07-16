using Microsoft.AspNetCore.Http;

namespace SimpleShop.Services.Interfaces;

public interface IImageService
{
    Task<string?> SaveImageAsync(IFormFile? imageFile);
}