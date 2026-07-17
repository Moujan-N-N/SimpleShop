using Microsoft.AspNetCore.Mvc;

namespace SimpleShop.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
