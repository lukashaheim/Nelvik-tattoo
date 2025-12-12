using Microsoft.AspNetCore.Mvc;
using Nelvik_Tattoo.Data;
using Nelvik_Tattoo.Models;

public class AdminBookingController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminBookingController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(DateOnly? from, DateOnly? to, string reset)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var q = _context.Bookings.AsQueryable();

        // if from is not selected, use "DateTime.Today"
        q = from.HasValue ? q.Where(b => b.SelectedDate >= from.Value) : q.Where(b => b.SelectedDate >= today);
        
        if (to.HasValue)
            q = q.Where(b => b.SelectedDate <= to.Value);
        
        ViewBag.From = from?.ToString("yyyy-MM-dd");
        ViewBag.To = to?.ToString("yyyy-MM-dd");
        
        return View(q.ToList());
    }
}