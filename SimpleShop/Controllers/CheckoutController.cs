using Microsoft.AspNetCore.Mvc;
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
    public IActionResult Index(
        CheckoutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }


        var cart = _cartService.GetCart();


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
            order.OrderItems.Add(
                new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
        }


        _context.Orders.Add(order);


        _context.SaveChanges();


        HttpContext.Session.Remove("Cart");


        return RedirectToAction(
            "Success",
            new { id = order.Id });
    }



    public IActionResult Success(int id)
    {
        ViewBag.OrderId = id;

        return View();
    }
}