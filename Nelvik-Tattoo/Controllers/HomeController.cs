using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nelvik_Tattoo.Data;
using Nelvik_Tattoo.Models;

namespace Nelvik_Tattoo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // --- HOME / INDEX ----------------------------------------------------
        public async Task<IActionResult> Index()
        {
            var featuredDesigns = await _db.GalleryDesigns
                .Where(g => g.IsFeatured)        // only show chosen photos
                .OrderByDescending(g => g.Id)
                .ToListAsync();

            // If none are featured yet, fall back to the latest 6 designs
            if (featuredDesigns.Count == 0)
            {
                featuredDesigns = await _db.GalleryDesigns
                    .OrderByDescending(g => g.Id)
                    .Take(8)
                    .ToListAsync();
            }
            
            return View(featuredDesigns);
        }


        // --- PRIVACY ---------------------------------------------------------
        public IActionResult Privacy()
        {
            return View();
        }

        // --- FAQ -------------------------------------------------------------
        public async Task<IActionResult> Faq()
        {
            var items = await _db.FaqItems
                .OrderBy(f => f.SortOrder)
                .ThenBy(f => f.Id)
                .ToListAsync();

            return View(items);
        }

        // --- ERROR -----------------------------------------------------------
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
