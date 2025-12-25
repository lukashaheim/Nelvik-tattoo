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
                var uploadsRoot = Path.Combine(_env.WebRootPath, "Images", "Booking", "Uploads");
                Directory.CreateDirectory(uploadsRoot);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.Photo.FileName)}";
                var filePath = Path.Combine(uploadsRoot, fileName);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await model.Photo.CopyToAsync(stream);

                model.ImagePath = $"/Images/Booking/Uploads/{fileName}";
            }

            _db.Bookings.Add(model);
            await _db.SaveChangesAsync();
            
            TempData["Date"] = model.SelectedDate.ToString("dd.MM.yyyy");
            TempData["Start"] = model.SelectedStartTime.ToString(@"HH\:mm");
            TempData["End"] = model.SelectedEndTime.ToString(@"HH\:mm");

            return RedirectToAction("Confirmation");
        }

        [HttpGet]
        public IActionResult Confirmation()
        {
            ViewBag.Date = TempData["Date"];
            ViewBag.Start = TempData["Start"];
            ViewBag.End = TempData["End"];

            ViewBag.StudioAddress = "Nelvik Tattoo, Dops gate 4, 0177 Oslo";

            return View();
        }
    }
}
