using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nelvik_Tattoo.Data;
using Nelvik_Tattoo.Models;

namespace Nelvik_Tattoo.Controllers
{
    [Authorize(Policy = "OwnerOnly")]
    [Route("theking/faq")]
    public class AdminFaqController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminFaqController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET /theking/faq
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var items = await _db.FaqItems
                .OrderBy(f => f.SortOrder)
                .ThenBy(f => f.Id)
                .ToListAsync();

            return View(items);
        }

// GET /theking/faq/create
        [HttpGet("create")]
        public IActionResult Create() => View();

// POST /theking/faq/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Question,Answer,SortOrder")] FaqItem model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.FaqItems.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        // GET: theking/faq/edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.FaqItems.FindAsync(id);
            if (item == null) return NotFound();

            return View(item);   // bruker Edit.cshtml (lager vi etterpå)
        }

// POST: theking/faq/edit/5
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer,SortOrder")] FaqItem model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            _db.Update(model);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET /theking/faq/delete/5  (bekreftelsesside – kan hoppe rett til POST hvis du vil)
        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.FaqItems.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }
// POST /theking/faq/delete/5
        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.FaqItems.FindAsync(id);
            if (item == null) return NotFound();

            _db.FaqItems.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}