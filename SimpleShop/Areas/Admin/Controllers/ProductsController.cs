using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}