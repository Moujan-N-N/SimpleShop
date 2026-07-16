namespace SimpleShop.Models.Entities;

public class Order
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;


    public decimal TotalAmount { get; set; }


    // Navigation Property
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}