namespace SimpleShop.Models.Cart;

public class ShoppingCart
{
    public List<CartItem> Items { get; set; } = new();


    public decimal TotalAmount =>
        Items.Sum(x => x.TotalPrice);



    public void AddItem(CartItem item)
    {
        var existingItem = Items
            .FirstOrDefault(x => x.ProductId == item.ProductId);


        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            Items.Add(item);
        }
    }



    public void IncreaseQuantity(int productId)
    {
        var item = Items
            .FirstOrDefault(x => x.ProductId == productId);


        if (item != null)
        {
            item.Quantity++;
        }
    }



    public void DecreaseQuantity(int productId)
    {
        var item = Items
            .FirstOrDefault(x => x.ProductId == productId);


        if (item != null)
        {
            item.Quantity--;


            if (item.Quantity <= 0)
            {
                Items.Remove(item);
            }
        }
    }



    public void RemoveItem(int productId)
    {
        var item = Items
            .FirstOrDefault(x => x.ProductId == productId);


        if (item != null)
        {
            Items.Remove(item);
        }
    }
}