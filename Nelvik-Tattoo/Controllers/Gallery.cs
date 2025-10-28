using Microsoft.AspNetCore.Mvc;

namespace Nelvik_Tattoo.Controllers;

public class Gallery : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}