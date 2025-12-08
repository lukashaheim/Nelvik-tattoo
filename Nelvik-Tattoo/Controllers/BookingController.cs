using Microsoft.AspNetCore.Mvc;
using Nelvik_Tattoo.Data;
using Nelvik_Tattoo.Models;

namespace Nelvik_Tattoo.Controllers
{
    public class BookingController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _db;

        public BookingController(IWebHostEnvironment env, ApplicationDbContext db)
        {
            _env = env;
            _db = db;
        }

        [HttpGet]
        public IActionResult Index() => View(new Booking());

        [HttpPost]
        public async Task<IActionResult> Index(Booking model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            if (model.Photo is { Length: > 0 })
            {
                var uploadsRoot = Path.Combine(
                    _env.WebRootPath,
                    "Images", "Booking", "Uploads");

                Directory.CreateDirectory(uploadsRoot);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.Photo.FileName)}";
                var filePath = Path.Combine(uploadsRoot, fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }
                
                model.ImagePath = $"/Images/Booking/Uploads/{fileName}";
            }
            
            _db.Bookings.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Confirmation));
        }

        [HttpGet]
        public IActionResult Confirmation() => View();
    }
}