using System.Text.Json;
using Microsoft.AspNetCore.Http;
using SimpleShop.Models.Cart;
using SimpleShop.Models.Entities;

namespace SimpleShop.Services;

public class CartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private const string CartSessionKey = "Cart";


    public CartService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }



    public ShoppingCart GetCart()
    {
        var session = _httpContextAccessor
            .HttpContext!
            .Session;


        var cartJson = session.GetString(CartSessionKey);


        if (string.IsNullOrEmpty(cartJson))
        {
            return new ShoppingCart();
        }


        return JsonSerializer.Deserialize<ShoppingCart>(cartJson)
               ?? new ShoppingCart();
    }



    public void AddToCart(Product product)
    {
        var cart = GetCart();


        var item = new CartItem
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Price = product.Price,
            Quantity = 1
        };


        cart.AddItem(item);


        SaveCart(cart);
    }



    public void RemoveFromCart(int productId)
    {
        var cart = GetCart();


        cart.RemoveItem(productId);


        SaveCart(cart);
    }

    public void IncreaseQuantity(int productId)
    {
        var cart = GetCart();


        cart.IncreaseQuantity(productId);


        SaveCart(cart);
    }



    public void DecreaseQuantity(int productId)
    {
        var cart = GetCart();


        cart.DecreaseQuantity(productId);


        SaveCart(cart);
    }


    private void SaveCart(ShoppingCart cart)
    {
        var session = _httpContextAccessor
            .HttpContext!
            .Session;


        var cartJson = JsonSerializer.Serialize(cart);


        session.SetString(
            CartSessionKey,
            cartJson);
    }


    public int GetCartCount()
    {
        var cart = GetCart();

        return cart.Items.Sum(x => x.Quantity);
    }
}