using Microsoft.AspNetCore.Mvc;

namespace EPR.Producer.PRN.Frontend.UI.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
