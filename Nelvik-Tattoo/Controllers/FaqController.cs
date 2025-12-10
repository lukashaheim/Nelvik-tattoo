using Microsoft.AspNetCore.Mvc;

namespace Nelvik_Tattoo.Controllers
{
    public class FaqController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}