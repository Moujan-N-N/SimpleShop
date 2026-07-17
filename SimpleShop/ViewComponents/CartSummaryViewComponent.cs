using Microsoft.AspNetCore.Mvc;
using SimpleShop.Services;

namespace SimpleShop.ViewComponents;

public class CartSummaryViewComponent : ViewComponent
{
    private readonly CartService _cartService;


    public CartSummaryViewComponent(
        CartService cartService)
    {
        _cartService = cartService;
    }



    public IViewComponentResult Invoke()
    {
        var count = _cartService.GetCartCount();

        return View(count);
    }
}