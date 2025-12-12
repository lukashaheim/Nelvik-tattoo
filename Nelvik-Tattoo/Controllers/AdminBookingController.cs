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

    public IActionResult Index(DateOnly? from, DateOnly? to, int page = 1)
    {
        const int pageSize = 10;
        
        var today = DateOnly.FromDateTime(DateTime.Today);
        
        var baseQ = _context.Bookings.AsQueryable();
        
        ViewBag.Count = baseQ.Count();

        // if from is not selected, use "DateTime.Today"
        var q = baseQ;
        q = from.HasValue ? 
            q.Where(b => b.SelectedDate >= from.Value) :
            q.Where(b => b.SelectedDate >= today);
        
        if (to.HasValue)
            q = q.Where(b => b.SelectedDate <= to.Value);
        
        var filteredCount = q.Count();
        ViewBag.FilteredCount = filteredCount;

        // Paging
        var totalPages = (int)Math.Ceiling(filteredCount / (double)pageSize);
        if (totalPages < 1) totalPages = 1;
        if (page < 1) page = 1;
        if  (page > totalPages) page = totalPages;
        
        ViewBag.Page = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.PageSize = pageSize;
        
        var list = q
            .OrderBy(b => b.SelectedDate)
            .ThenBy(b => b.SelectedStartTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        var fromRow = filteredCount == 0 ? 0 : ((page - 1) * pageSize) + 1;
        var toRow   = Math.Min(page * pageSize, filteredCount);
        
        ViewBag.FromRow = fromRow;
        ViewBag.ToRow = toRow;

        ViewBag.ShownCount = list.Count;
        
        ViewBag.From = from?.ToString("yyyy-MM-dd");
        ViewBag.To = to?.ToString("yyyy-MM-dd");
        
        return View(list);
    }
}