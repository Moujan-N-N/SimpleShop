using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleShop.Models.Entities;
using SimpleShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleShop.Data;
using SimpleShop.ViewModels.Product;

namespace SimpleShop.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ApplicationDbContext _context;


    public ProductsController(
     IProductService productService,
     ApplicationDbContext context)
    {
        _productService = productService;
        _context = context;
    }



    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllAsync();

        return View(products);
    }



    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new CreateProductViewModel();

        viewModel.Categories = await _context.Categories
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToListAsync();

        var categories = await _context.Categories.ToListAsync();

        var count = categories.Count;

        return View(viewModel);
    }



    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }


        await _productService.AddAsync(product);


        return RedirectToAction(nameof(Index));
    }
}