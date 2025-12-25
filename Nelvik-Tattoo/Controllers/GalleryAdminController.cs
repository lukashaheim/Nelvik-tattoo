using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nelvik_Tattoo.Data;
using Nelvik_Tattoo.Models;

namespace Nelvik_Tattoo.Controllers
{
    [Authorize]
    [Route("theking/gallery")]
    public class GalleryAdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public GalleryAdminController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: /theking/gallery
        [HttpGet("")]
        public IActionResult Index()
        {
            var designs = _db.GalleryDesigns
                .OrderByDescending(d => d.Id)
                .ToList();

            return View(designs);
        }

        // GET: /theking/gallery/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /theking/gallery/create
        [HttpPost("create")]
        public IActionResult Create(GalleryDesign design, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
                return View(design);
            
            // Sørg for at de ikke er begge true
            if (design.IsFlash)
                design.IsFinished = false;
            else if (design.IsFinished)
                design.IsFlash = false;
            
            if (imageFile != null)
            {
                string folder = design.IsFlash
                    ? "Images/Gallery/Flash"
                    : "Images/Gallery/Finished";

                string folderPath = Path.Combine(_env.WebRootPath, folder);
                Directory.CreateDirectory(folderPath);

                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                imageFile.CopyTo(stream);

                design.ImagePath = "/" + folder + "/" + fileName;
            }

            _db.GalleryDesigns.Add(design);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: /theking/gallery/edit/5
        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var design = _db.GalleryDesigns.Find(id);
            if (design == null)
                return NotFound();

            return View(design);
        }

        // POST: /theking/gallery/edit/5
        [HttpPost("edit/{id}")]
        public IActionResult Edit(int id, GalleryDesign model, IFormFile? imageFile)
        {
            var design = _db.GalleryDesigns.Find(id);
            if (design == null)
                return NotFound();

            // Oppdater felter
            design.Title = model.Title;
            design.Description = model.Description;
            design.Price = model.Price;
            design.IsFlash = model.IsFlash;
            design.IsFinished = model.IsFinished;
            design.Placement = model.Placement;
            
            // Sørg for at de ikke er begge true
            if (design.IsFlash)
                design.IsFinished = false;
            else if (design.IsFinished)
                design.IsFlash = false;

            // Nytt bilde lastet opp?
            if (imageFile != null)
            {
                // Slett gammel fil hvis den finnes
                if (!string.IsNullOrEmpty(design.ImagePath))
                {
                    string oldFile = Path.Combine(_env.WebRootPath, design.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFile))
                        System.IO.File.Delete(oldFile);
                }

                // Velg riktig mappe
                string folder = model.IsFlash
                    ? "Images/Gallery/Flash"
                    : "Images/Gallery/Finished";

                string folderPath = Path.Combine(_env.WebRootPath, folder);
                Directory.CreateDirectory(folderPath);

                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                imageFile.CopyTo(stream);

                design.ImagePath = "/" + folder + "/" + fileName;
            }

            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: /theking/gallery/delete/5
        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var design = _db.GalleryDesigns.Find(id);
            if (design == null)
                return NotFound();

            return View(design);
        }

        // POST: /theking/gallery/delete/5
        [HttpPost("delete/{id}")]
        [ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            var design = _db.GalleryDesigns.Find(id);
            if (design == null)
                return NotFound();

            // Slett bilde fra disk
            if (!string.IsNullOrEmpty(design.ImagePath))
            {
                string oldFile = Path.Combine(_env.WebRootPath, design.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldFile))
                    System.IO.File.Delete(oldFile);
            }

            _db.GalleryDesigns.Remove(design);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
