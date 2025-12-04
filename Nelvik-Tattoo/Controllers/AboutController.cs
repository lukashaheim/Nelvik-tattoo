
using Microsoft.AspNetCore.Mvc;

namespace Nelvik_Tattoo.Controllers;
public class AboutController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}