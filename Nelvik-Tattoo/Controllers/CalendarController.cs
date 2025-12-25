using Microsoft.AspNetCore.Mvc;
using System.Text;
using Nelvik_Tattoo.Data;

public class CalendarController : Controller
{
    private readonly ApplicationDbContext _context;

    public CalendarController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("calendar/bookings.ics")]
    public IActionResult BookingsCalendar()
    {
        var bookings = _context.Bookings.ToList();

        var sb = new StringBuilder();
        sb.AppendLine("BEGIN:VCALENDAR");
        sb.AppendLine("VERSION:2.0");
        sb.AppendLine("PRODID:-//Nelvik Tattoo//Bookings//EN");

        foreach (var b in bookings)
        {
            var start = b.SelectedDate.ToDateTime(b.SelectedStartTime);
            var end = b.SelectedDate.ToDateTime(b.SelectedEndTime);

            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine($"UID:booking-{b.Id}@nelvik-tattoo");
            sb.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
            sb.AppendLine($"DTSTART:{start:yyyyMMddTHHmmss}");
            sb.AppendLine($"DTEND:{end:yyyyMMddTHHmmss}");
            sb.AppendLine($"SUMMARY:Tattoo booking");
            sb.AppendLine($"DESCRIPTION:{b.Design} – {b.Placement}");
            sb.AppendLine("END:VEVENT");
        }

        sb.AppendLine("END:VCALENDAR");

        return File(
            Encoding.UTF8.GetBytes(sb.ToString()),
            "text/calendar",
            "bookings.ics"
        );
    }
}