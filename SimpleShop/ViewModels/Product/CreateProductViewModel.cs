using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SimpleShop.ViewModels.Product;

public class CreateProductViewModel
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Range(1, 1000000000)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, 100000)]
    public int Stock { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public IFormFile? ImageFile { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; }
        = Enumerable.Empty<SelectListItem>();
}