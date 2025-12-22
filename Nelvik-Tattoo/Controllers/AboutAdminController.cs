using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nelvik_Tattoo.Data;
using Nelvik_Tattoo.Models;

namespace Nelvik_Tattoo.Controllers
{
    [Authorize]
    [Route("theking/about")]
    public class AboutAdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AboutAdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET /theking/about
        [HttpGet("")]
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

        // GET /theking/about/edit
        [HttpGet("edit")]
        public async Task<IActionResult> Edit()
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

        // POST /theking/about/edit
        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AboutPage model, IFormFile ImageFile)
        {
            
            var page = await _db.AboutPages.FirstOrDefaultAsync();

            if (page == null)
            {
                page = new AboutPage();
                _db.AboutPages.Add(page);
            }

            page.Title = model.Title;
            page.Body = model.Body;

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

                page.ImagePath = "/Images/" + fileName;
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
