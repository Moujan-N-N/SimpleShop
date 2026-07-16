namespace SimpleShop.Models.Entities
{
    public class Category
    { 
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;


        // Navigation Property
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
