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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            return View(viewModel);
        }

        var product = new Product
        {
            Name = viewModel.Name,
            Description = viewModel.Description,
            Price = viewModel.Price,
            Stock = viewModel.Stock,
            CategoryId = viewModel.CategoryId
        };

        if (viewModel.ImageFile != null)
        {
            var fileName = Guid.NewGuid().ToString()
                + Path.GetExtension(viewModel.ImageFile.FileName);

            var uploadPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/images/products"
            );

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await viewModel.ImageFile.CopyToAsync(stream);
            }

            product.ImageUrl = "/images/products/" + fileName;
        }

        await _productService.AddAsync(product);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }



    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        ViewBag.Categories = new SelectList(
            await _context.Categories.ToListAsync(),
            "Id",
            "Name",
            product.CategoryId
        );

        return View(product);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
     int id,
     Product product,
     IFormFile? ImageFile)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(
                await _context.Categories.ToListAsync(),
                "Id",
                "Name",
                product.CategoryId
            );

            return View(product);
        }

        var existingProduct = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (existingProduct == null)
        {
            return NotFound();
        }

        if (ImageFile != null)
        {
            // Delete old image
            if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
            {
                var oldImagePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    existingProduct.ImageUrl.TrimStart('/')
                );

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Save new image
            var fileName = Guid.NewGuid().ToString()
                + Path.GetExtension(ImageFile.FileName);

            var uploadPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/images/products"
            );

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(
                filePath,
                FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            product.ImageUrl = "/images/products/" + fileName;
        }
        else
        {
            // Keep old image
            product.ImageUrl = existingProduct.ImageUrl;
        }

        _context.Products.Update(product);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        // Delete image from wwwroot
        if (!string.IsNullOrEmpty(product.ImageUrl))
        {
            var imagePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                product.ImageUrl.TrimStart('/')
            );

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        // Delete product from database
        _context.Products.Remove(product);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }










}