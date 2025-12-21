using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Create(GalleryDesign design, List<IFormFile> imageFiles)
        {
            if (!ModelState.IsValid)
                return View(design);
            
            // Sørg for at de ikke er begge true
            if (design.IsFlash)
                design.IsFinished = false;
            else if (design.IsFinished)
                design.IsFlash = false;
            
            // riktig mappe (samme som du gjør i dag)
            string folder = design.IsFlash ? "Images/Gallery/Flash" : "Images/Gallery/Finished";
            string folderPath = Path.Combine(_env.WebRootPath, folder);
            Directory.CreateDirectory(folderPath);

            int order = 0;
            foreach (var file in imageFiles.Where(f => f != null && f.Length > 0))
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);

                var path = "/" + folder + "/" + fileName;

                // sett cover image til første bilde, for bakoverkompatibilitet
                if (string.IsNullOrEmpty(design.ImagePath))
                    design.ImagePath = path;

                _db.GalleryDesignImages.Add(new GalleryDesignImage
                {
                    GalleryDesignId = design.Id,
                    ImagePath = path,
                    SortOrder = order++
                });
            }

            _db.GalleryDesigns.Add(design);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: /theking/gallery/edit/5
        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var design = _db.GalleryDesigns
                .Include(d => d.Images)
                .FirstOrDefault(d => d.Id == id);

            if (design == null)
                return NotFound();
            
            if (!string.IsNullOrEmpty(design.ImagePath) &&
                    !design.Images.Any(i => i.ImagePath == design.ImagePath))
                {
                    design.Images.Add(new GalleryDesignImage
                    {
                        GalleryDesignId = design.Id,
                        ImagePath = design.ImagePath,
                        SortOrder = 0
                    });
                    _db.SaveChanges();
                }
            return View(design);
        }

        // POST: /theking/gallery/edit/5
        [HttpPost("edit/{id}")]
        public IActionResult Edit(int id, GalleryDesign model, List<IFormFile>? imageFiles)
        {
            var design = _db.GalleryDesigns
                .Include(d => d.Images)
                .FirstOrDefault(d => d.Id == id);

            if (design == null)
                return NotFound();

            // Oppdater felter
            design.Title = model.Title;
            design.Description = model.Description;
            design.Price = model.Price;
            design.IsFlash = model.IsFlash;
            design.IsFinished = model.IsFinished;
            design.Placement = model.Placement;

            // Sørg for at de ikke er begge true (samme logikk som du har)
            if (design.IsFlash) design.IsFinished = false;
            else if (design.IsFinished) design.IsFlash = false;

            // ✅ LEGG TIL nye bilder (ikke slett gamle)
            if (imageFiles != null && imageFiles.Count > 0)
            {
                string folder = design.IsFlash ? "Images/Gallery/Flash" : "Images/Gallery/Finished";
                string folderPath = Path.Combine(_env.WebRootPath, folder);
                Directory.CreateDirectory(folderPath);

                int nextOrder = (design.Images.Any() ? design.Images.Max(i => i.SortOrder) + 1 : 0);

                foreach (var file in imageFiles.Where(f => f != null && f.Length > 0))
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(stream);

                    var path = "/" + folder + "/" + fileName;

                    // behold ImagePath som "cover" hvis den er tom
                    if (string.IsNullOrEmpty(design.ImagePath))
                        design.ImagePath = path;

                    design.Images.Add(new GalleryDesignImage
                    {
                        ImagePath = path,
                        SortOrder = nextOrder++
                    });
                }
            }

            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost("delete-image/{imageId}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteImage(int imageId)
        {
            var img = _db.GalleryDesignImages
                .Include(i => i.GalleryDesign)
                .Include(i => i.GalleryDesign.Images)
                .FirstOrDefault(i => i.Id == imageId);

            if (img == null)
                return NotFound();

            var design = img.GalleryDesign;

            // slett fil fra disk
            if (!string.IsNullOrEmpty(img.ImagePath))
            {
                string fullPath = Path.Combine(_env.WebRootPath, img.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }

            _db.GalleryDesignImages.Remove(img);
            _db.SaveChanges();

            // hvis cover-bildet var dette, sett ny cover (eller null)
            if (design.ImagePath == img.ImagePath)
            {
                var newCover = _db.GalleryDesignImages
                    .Where(i => i.GalleryDesignId == design.Id)
                    .OrderBy(i => i.SortOrder)
                    .Select(i => i.ImagePath)
                    .FirstOrDefault();

                design.ImagePath = newCover; // kan bli null hvis ingen igjen
                _db.SaveChanges();
            }

            return RedirectToAction("Edit", new { id = design.Id });
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
            var design = _db.GalleryDesigns
                .Include(d => d.Images)
                .FirstOrDefault(d => d.Id == id);

            if (design == null)
                return NotFound();

            // delete all image files (cover + additional)
            var paths = new List<string>();

            if (!string.IsNullOrEmpty(design.ImagePath))
                paths.Add(design.ImagePath);

            if (design.Images != null && design.Images.Count > 0)
                paths.AddRange(design.Images.Select(i => i.ImagePath));

            foreach (var p in paths.Distinct())
            {
                var fullPath = Path.Combine(_env.WebRootPath, p.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }

            // delete image rows then design
            _db.GalleryDesignImages.RemoveRange(design.Images);
            _db.GalleryDesigns.Remove(design);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost("set-cover/{imageId}")]
        [ValidateAntiForgeryToken]
        public IActionResult SetCover(int imageId)
        {
            var img = _db.GalleryDesignImages
                .Include(i => i.GalleryDesign)
                .FirstOrDefault(i => i.Id == imageId);

            if (img == null)
                return NotFound();

            img.GalleryDesign.ImagePath = img.ImagePath; // set cover
            _db.SaveChanges();

            return RedirectToAction("Edit", new { id = img.GalleryDesignId });
        }
    }
}
