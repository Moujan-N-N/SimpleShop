using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleShop.Models.Entities;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductsController : Controller
{
    private readonly IProductService _productService;


    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }



    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllAsync();

        return View(products);
    }



    [HttpGet]
    public IActionResult Create()
    {
        return View();
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