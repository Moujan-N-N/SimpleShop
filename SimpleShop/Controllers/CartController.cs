using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleShop.Data;
using SimpleShop.Services;

namespace SimpleShop.Controllers;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly CartService _cartService;


    public CartController(
        ApplicationDbContext context,
        CartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }



    public IActionResult Index()
    {
        var cart = _cartService.GetCart();

        return View(cart);
    }



    public async Task<IActionResult> Add(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id);


        if (product == null)
        {
            return NotFound();
        }


        _cartService.AddToCart(product);


        return RedirectToAction(
            "Index",
            "Cart");
    }



    public IActionResult Remove(int id)
    {
        _cartService.RemoveFromCart(id);


        return RedirectToAction(
            "Index");
    }

    public IActionResult Increase(int id)
    {
        _cartService.IncreaseQuantity(id);


        return RedirectToAction("Index");
    }



    public IActionResult Decrease(int id)
    {
        _cartService.DecreaseQuantity(id);


        return RedirectToAction("Index");
    }
}