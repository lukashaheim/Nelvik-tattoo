using Microsoft.AspNetCore.Mvc;
using Nelvik_Tattoo.Data;
using Nelvik_Tattoo.Models;

namespace Nelvik_Tattoo.Controllers
{
    public class GalleryAdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public GalleryAdminController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // LISTE OVER ALLE MOTIVER
        public IActionResult Index()
        {
            var designs = _db.GalleryDesigns
                .OrderByDescending(d => d.Id)
                .ToList();

            return View(designs);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        public IActionResult Create(GalleryDesign design, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
                return View(design);

            if (imageFile != null)
            {
                // Velg riktig undermappe basert på IsFlash
                var folder = design.IsFlash
                    ? "Images/Gallery/Flash"
                    : "Images/Gallery/Finished";

                var folderPath = Path.Combine(_env.WebRootPath, folder);
                Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                imageFile.CopyTo(stream);

                design.ImagePath = "/" + folder + "/" + fileName;
            }
            
            _db.GalleryDesigns.Add(design);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {
            var design = _db.GalleryDesigns.Find(id);
            if (design == null)
                return NotFound();

            return View(design);
        }

        // POST: Edit
        [HttpPost]
        public IActionResult Edit(GalleryDesign model, IFormFile? imageFile)
        {
            var design = _db.GalleryDesigns.Find(model.Id);
            if (design == null)
                return NotFound();

            design.Title = model.Title;
            design.Description = model.Description;
            design.Price = model.Price;
            design.IsFlash = model.IsFlash;

            if (imageFile != null)
            {
                // (valgfritt) Slett gammelt bilde
                if (!string.IsNullOrEmpty(design.ImagePath))
                {
                    var oldFile = Path.Combine(_env.WebRootPath, design.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFile))
                        System.IO.File.Delete(oldFile);
                }

                // Velg riktig undermappe basert på IsFlash etter redigering
                var folder = model.IsFlash
                    ? "Images/Gallery/Flash"
                    : "Images/Gallery/Finished";

                var folderPath = Path.Combine(_env.WebRootPath, folder);
                Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                imageFile.CopyTo(stream);

                design.ImagePath = "/" + folder + "/" + fileName;
            }


            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: Delete
        public IActionResult Delete(int id)
        {
            var design = _db.GalleryDesigns.Find(id);
            if (design == null)
                return NotFound();

            return View(design);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var design = _db.GalleryDesigns.Find(id);
            if (design == null)
                return NotFound();

            _db.GalleryDesigns.Remove(design);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
