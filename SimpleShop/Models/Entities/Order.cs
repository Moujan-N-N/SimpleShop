namespace SimpleShop.Models.Entities;

public class Order
{
    public int Id { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.Now;


    public decimal TotalAmount { get; set; }


    // Customer Information

    public string CustomerName { get; set; } = string.Empty;


    public string PhoneNumber { get; set; } = string.Empty;


    public string Address { get; set; } = string.Empty;



    // Navigation Property

    public ICollection<OrderItem> OrderItems { get; set; }
        = new List<OrderItem>();
}