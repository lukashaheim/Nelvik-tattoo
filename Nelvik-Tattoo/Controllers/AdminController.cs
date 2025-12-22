using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nelvik_Tattoo.Controllers
{
    [Authorize]
    [Route("theking")]                 // admin-side nå på /theking (ikke /admin)
    public class AdminController : Controller
    {
        [HttpGet("")]
        public IActionResult Index() => View();
    }
}