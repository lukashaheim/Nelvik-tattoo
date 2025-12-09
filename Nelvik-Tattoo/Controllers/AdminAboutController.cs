using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nelvik_Tattoo.Data;
using Nelvik_Tattoo.Models;

namespace Nelvik_Tattoo.Controllers
{
    [Authorize(Policy = "OwnerOnly")]
    [Route("theking/about")]
    public class AdminAboutController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminAboutController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET /theking/about
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            // We assume there is only ONE AboutPage row
            var page = await _db.AboutPages.FirstOrDefaultAsync();

            // If none exists yet, create a default one
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

        // GET /theking/about/edit
        [HttpGet("edit")]
        public async Task<IActionResult> Edit()
        {
            var page = await _db.AboutPages.FirstOrDefaultAsync();

            if (page == null)
            {
                // If somehow missing, show an empty model
                page = new AboutPage();
            }

            return View(page);
        }

        // POST /theking/about/edit
        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AboutPage model, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Load existing row (we expect only one)
            var page = await _db.AboutPages.FirstOrDefaultAsync(p => p.Id == model.Id);

            if (page == null)
            {
                // If it doesn't exist yet, create it
                page = new AboutPage();
                _db.AboutPages.Add(page);
            }

            page.Title = model.Title;
            page.Body = model.Body;

            // Handle optional image upload
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var folderPath = Path.Combine("wwwroot", "Images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // URL used by the site
                page.ImagePath = "/Images/" + fileName;
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
