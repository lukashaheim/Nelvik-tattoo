using Microsoft.AspNetCore.Mvc;
using Nelvik_Tattoo.Models;

namespace Nelvik_Tattoo.Controllers;

public class BookingController : Controller
{
    private readonly IWebHostEnvironment _env;
    public BookingController(IWebHostEnvironment env) => _env = env;

    [HttpGet]
    public IActionResult Index() => View(new Booking());
    
    [HttpGet]
    public IActionResult Confirmation() => View();
    
    [HttpPost]
    public async Task<IActionResult> Index(Booking model)
    {
        if (!ModelState.IsValid)
            return View(model);
        
        return RedirectToAction(nameof(Confirmation));
    }
}