using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nelvik_Tattoo.Data;
using Nelvik_Tattoo.Models;

namespace YourProject.Controllers
{
    // [Authorize(Roles = "Admin")]
    public class FlashAdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public FlashAdminController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: /FlashAdmin
        public IActionResult Index()
        {
            var flashes = _db.FlashDesigns.ToList();
            return View(flashes);
        }

        // GET: /FlashAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /FlashAdmin/Create
        [HttpPost]
        public IActionResult Create(FlashDesign flash, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
                return View(flash);

            if (imageFile != null)
            {
                // Lag en unik filsti
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var folderPath = Path.Combine(_env.WebRootPath, "Images/Gallery");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                // Sett ImagePath så galleriet kan finne den
                flash.ImagePath = "/Images/Gallery/" + fileName;
            }

            _db.FlashDesigns.Add(flash);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: /FlashAdmin/Edit/5
        public IActionResult Edit(int id)
        {
            var flash = _db.FlashDesigns.Find(id);
            if (flash == null)
                return NotFound();

            return View(flash);
        }

        // POST: /FlashAdmin/Edit/5
        [HttpPost]
        public IActionResult Edit(FlashDesign flash, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
                return View(flash);

            var existing = _db.FlashDesigns.Find(flash.Id);
            if (existing == null)
                return NotFound();

            // Oppdater verdier
            existing.Title = flash.Title;
            existing.Description = flash.Description;
            existing.Price = flash.Price;
            existing.IsAvailable = flash.IsAvailable;

            // Hvis nytt bilde lastes opp
            if (imageFile != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var folderPath = Path.Combine(_env.WebRootPath, "Images/Gallery");
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    imageFile.CopyTo(stream);

                existing.ImagePath = "/Images/Gallery/" + fileName;
            }

            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: /FlashAdmin/Delete/5
        public IActionResult Delete(int id)
        {
            var flash = _db.FlashDesigns.Find(id);
            if (flash == null)
                return NotFound();

            return View(flash);
        }

        // POST: /FlashAdmin/DeleteConfirmed
        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            var flash = _db.FlashDesigns.Find(id);
            if (flash == null)
                return NotFound();

            _db.FlashDesigns.Remove(flash);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
