using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nelvik_Tattoo.Data;
using Nelvik_Tattoo.Models;

namespace Nelvik_Tattoo.Controllers
{
    public class GalleryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GalleryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------------------------------
        // LISTE OVER ALLE MOTIVER (BLANDET)
        // -------------------------------------------
        public async Task<IActionResult> Index(string? placement)
        {
            // Flashes: always shown, not filtered
            var flashes = await _context.GalleryDesigns
                .Where(d => d.IsFlash)
                .OrderByDescending(d => d.Id)
                .ToListAsync();

            // Non-flash designs
            var query = _context.GalleryDesigns
                .Where(d => !d.IsFlash);

            if (!string.IsNullOrWhiteSpace(placement))
            {
                query = query.Where(d => d.Placement == placement);
            }

            var designs = await query
                .OrderByDescending(d => d.Id)
                .ToListAsync();

            var placements = await _context.GalleryDesigns
                .Where(d => !d.IsFlash && !string.IsNullOrEmpty(d.Placement))
                .Select(d => d.Placement!)
                .Distinct()
                .OrderBy(p => p)
                .ToListAsync();

            ViewBag.Flashes = flashes;
            ViewBag.Placements = placements;
            ViewBag.SelectedPlacement = placement;

            // Model = only non-flash designs
            return View(designs);
        }



        // -------------------------------------------
        // KUN FLASH – Til salgs
        // -------------------------------------------
        public async Task<IActionResult> Flashes()
        {
            var flashes = await _context.GalleryDesigns
                .Where(d => d.IsFlash)
                .OrderByDescending(d => d.Id)
                .ToListAsync();

            return View(flashes);
        }

        // -------------------------------------------
        // DETALJER FOR ETT MOTIV
        // -------------------------------------------
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
                return BadRequest();

            var design = await _context.GalleryDesigns
                .FirstOrDefaultAsync(d => d.Id == id);

            if (design == null)
                return NotFound();

            return View(design);
        }
    }
}
