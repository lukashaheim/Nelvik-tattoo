using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nelvik_Tattoo.Data;

namespace Nelvik_Tattoo.Controllers
{
    [Authorize(Policy = "OwnerOnly")]
    [Route("theking/home")]
    public class HomeAdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeAdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET /theking/home
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            // Show a simple overview: which images are featured, etc.
            var designs = await _db.GalleryDesigns
                .OrderByDescending(g => g.Id)
                .ToListAsync();

            return View(designs);   // Views/HomeAdmin/Index.cshtml
        }

        // GET /theking/home/edit
        [HttpGet("edit")]
        public async Task<IActionResult> Edit()
        {
            // Same data but in "edit mode" (checkboxes/buttons)
            var designs = await _db.GalleryDesigns
                .OrderByDescending(g => g.Id)
                .ToListAsync();

            return View(designs);   // Views/HomeAdmin/Edit.cshtml
        }

        // POST /theking/home/edit
        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int[] featuredIds)
        {
            // Clear all first
            var allDesigns = await _db.GalleryDesigns.ToListAsync();

            foreach (var d in allDesigns)
            {
                d.IsFeatured = featuredIds.Contains(d.Id);
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}