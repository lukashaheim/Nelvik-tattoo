using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nelvik_Tattoo.Controllers
{
    [Authorize] // krever innlogging
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}