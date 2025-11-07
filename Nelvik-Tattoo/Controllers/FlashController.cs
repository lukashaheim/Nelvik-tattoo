using Microsoft.AspNetCore.Mvc;
using Nelvik_Tattoo.Models;

namespace Nelvik_Tattoo.Controllers;

public class FlashController : Controller
{
    public IActionResult Index() => View();
}