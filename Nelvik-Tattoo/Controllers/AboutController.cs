using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nelvik_Tattoo.Data;
using Nelvik_Tattoo.Models;

namespace Nelvik_Tattoo.Controllers
{
    public class AboutController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AboutController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET /about
        [HttpGet("/about")]
        public async Task<IActionResult> Index()
        {
            var page = await _db.AboutPages.FirstOrDefaultAsync();

            if (page == null)
            {
                page = new AboutPage
                {
                    Title = "About Nikolai Nelvik",
                    Body = "Write your about text here.",
                    ImagePath = "/Images/about.jpg"
                };

                _db.AboutPages.Add(page);
                await _db.SaveChangesAsync();
            }

            return View(page);
        }
    }
}