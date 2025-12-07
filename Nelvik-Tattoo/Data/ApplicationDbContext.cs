using Nelvik_Tattoo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Nelvik_Tattoo.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Console.WriteLine(">>> RUNNING Nelvik_Tattoo ApplicationDbContext <<<");
    }

    public DbSet<Booking> Bookings { get; set; }
    public DbSet<GalleryDesign> GalleryDesigns { get; set; }
    
    public DbSet<FlashDesign> FlashDesigns => Set<FlashDesign>();

}
