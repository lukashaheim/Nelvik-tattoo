using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nelvik_Tattoo.Data;

namespace Nelvik_Tattoo.Controllers
{
    public class FaqController : Controller
    {
        private readonly ApplicationDbContext _db;
        public FaqController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var items = await _db.FaqItems
                .OrderBy(f => f.SortOrder)
                .ThenBy(f => f.Id)
                .ToListAsync();

            return View(items);
        }
        
    }
}