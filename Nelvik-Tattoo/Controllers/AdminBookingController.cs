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
    
    public IActionResult Edit(int id)
    {
        var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
        if (booking == null) return NotFound();
        return View(booking);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Booking model)
    {
        if (!ModelState.IsValid) return View(model);

        var booking = _context.Bookings.FirstOrDefault(b => b.Id == model.Id);
        if (booking == null) return NotFound();

        booking.SelectedDate = model.SelectedDate;
        booking.SelectedStartTime = model.SelectedStartTime;
        booking.SelectedEndTime = model.SelectedEndTime;
        booking.Email = model.Email;
        booking.Phone = model.Phone;
        booking.PrefferedCM = model.PrefferedCM;
        booking.Size = model.Size;
        booking.Design = model.Design;
        booking.Placement = model.Placement;
        booking.Budget = model.Budget;
        booking.Comment = model.Comment;

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }


    public IActionResult Index(DateOnly? from, DateOnly? to, int page = 1)
    {
        const int pageSize = 10;

        var now = DateTime.Now;
        var nowDate = DateOnly.FromDateTime(now);
        var nowTime = TimeOnly.FromDateTime(now);

        var baseQ = _context.Bookings.AsQueryable();
        ViewBag.Count = baseQ.Count();

        var q = baseQ;

        if (!from.HasValue)
        {
            q = q.Where(b =>
                b.SelectedDate > nowDate ||
                (b.SelectedDate == nowDate && b.SelectedEndTime > nowTime)
            );
        }
        else
        {
            q = q.Where(b => b.SelectedDate >= from.Value);
        }

        if (to.HasValue)
            q = q.Where(b => b.SelectedDate <= to.Value);

        var filteredCount = q.Count();
        ViewBag.FilteredCount = filteredCount;
        
        var allFiltered = q
            .Select(b => new { b.Id, b.SelectedDate, b.SelectedStartTime, b.SelectedEndTime })
            .ToList();

        var collisionIds = new HashSet<int>();

        foreach (var dayGroup in allFiltered.GroupBy(b => b.SelectedDate))
        {
            var dayList = dayGroup
                .OrderBy(b => b.SelectedStartTime)
                .ToList();

            for (int i = 0; i < dayList.Count; i++)
            {
                var a = dayList[i];

                for (int j = i + 1; j < dayList.Count; j++)
                {
                    var b = dayList[j];
                    
                    bool overlaps = a.SelectedStartTime < b.SelectedEndTime &&
                                    a.SelectedEndTime > b.SelectedStartTime;

                    if (overlaps)
                    {
                        collisionIds.Add(a.Id);
                        collisionIds.Add(b.Id);
                    }
                }
            }
        }

        ViewBag.CollisionIds = collisionIds;
        
        var totalPages = (int)Math.Ceiling(filteredCount / (double)pageSize);
        if (totalPages < 1) totalPages = 1;
        if (page < 1) page = 1;
        if (page > totalPages) page = totalPages;

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
        var toRow = Math.Min(page * pageSize, filteredCount);

        ViewBag.FromRow = fromRow;
        ViewBag.ToRow = toRow;

        ViewBag.ShownCount = list.Count;

        ViewBag.From = from?.ToString("yyyy-MM-dd");
        ViewBag.To = to?.ToString("yyyy-MM-dd");

        return View(list);
    }
}
