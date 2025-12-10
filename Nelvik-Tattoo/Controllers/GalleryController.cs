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
        public async Task<IActionResult> Index()
        {
            var designs = await _context.GalleryDesigns
                .OrderByDescending(d => d.Id)
                .ToListAsync();

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
