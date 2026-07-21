using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleShop.Data;

namespace SimpleShop.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var productCount = await _context.Products.CountAsync();

        var categoryCount = await _context.Category.CountAsync();

        var userCount = await _context.Users.CountAsync();

        var orderCount = await _context.Orders.CountAsync();

        ViewBag.ProductCount = productCount;
        ViewBag.CategoryCount = categoryCount;
        ViewBag.UserCount = userCount;
        ViewBag.OrderCount = orderCount;

        return View();
    }
}