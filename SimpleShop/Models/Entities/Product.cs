namespace SimpleShop.Models.Entities;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;


    // Foreign Key
    public int CategoryId { get; set; }

    // Navigation Property
    public Category? Category { get; set; }
}