using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleShop.Data;
using SimpleShop.Models.Entities;
using SimpleShop.Models.ViewModels;
using SimpleShop.Services;
using System.Security.Claims;

namespace SimpleShop.Controllers;

public class CheckoutController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly CartService _cartService;


    public CheckoutController(
        ApplicationDbContext context,
        CartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }


    [HttpGet]
    public IActionResult Index()
    {
        var cart = _cartService.GetCart();


        if (!cart.Items.Any())
        {
            return RedirectToAction(
                "Index",
                "Cart");
        }


        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        CheckoutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }


        var cart = _cartService.GetCart();


        if (!cart.Items.Any())
        {
            return RedirectToAction(
                "Index",
                "Cart");
        }


        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);


        var order = new Order
        {
            UserId = userId,

            CustomerName = model.CustomerName,

            PhoneNumber = model.PhoneNumber,

            Address = model.Address,

            CreatedAt = DateTime.Now,

            TotalAmount = cart.TotalAmount
        };


        foreach (var item in cart.Items)
        {
            var product = await _context.Products
                .FindAsync(item.ProductId);

            if (product == null)
            {
                return NotFound();
            }

            if (product.Stock < item.Quantity)
            {
                ModelState.AddModelError(
                    "",
                    $"Not enough stock for {product.Name}.");

                return View(model);
            }

            product.Stock -= item.Quantity;

            order.OrderItems.Add(
                new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
        }


        _context.Orders.Add(order);


        await _context.SaveChangesAsync();


        HttpContext.Session.Remove("Cart");


        return RedirectToAction(
            "Success",
            new
            {
                id = order.Id
            });
    }


    public IActionResult Success(int id)
    {
        ViewBag.OrderId = id;


        return View();
    }
}