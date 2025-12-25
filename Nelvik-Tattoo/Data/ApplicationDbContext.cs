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
    public DbSet<GalleryDesignImage> GalleryDesignImages { get; set; }
    public DbSet<FaqItem> FaqItems { get; set; }
    public DbSet<AboutPage> AboutPages { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FaqItem>().HasData(
            new FaqItem
            {
                Id = 1,
                SortOrder = 1,
                Question = "How much does a tattoo cost?",
                Answer = "The price of a tattoo varies depending on size, placement, and level of detail."
            },
            new FaqItem
            {
                Id = 2,
                SortOrder = 2,
                Question = "Do you require a deposit?",
                Answer = "A deposit of NOK 1000 is required before an appointment can be confirmed. This deposit is non-refundable and will be deducted from the total price of the tattoo.\n"
            },
            new FaqItem
            {
                Id = 3,
                SortOrder = 3,
                Question = "How should I prepare for my appointment?",
                Answer = "Before your tattoo appointment, make sure you are well-rested and have eaten and hydrated properly. Feel free to bring something sugary in case you need to boost your blood sugar during the session."
            },
            new FaqItem
            {
                Id = 4,
                SortOrder = 4,
                Question = "How do I take care of my tattoo?",
                Answer = "If your tattoo is covered with second skin, you can keep it on for up to three days. After removing it, gently wash the tattoo with a mild, fragrance-free soap and apply an unscented moisturizer.\n"
            },
            new FaqItem
            {
                Id = 5,
                SortOrder = 5,
                Question = "Can I book a consultation?",
                Answer = "Yes. Consultations are free and can help you and your tattoo artist figure out the design, placement, and details."
            },
            new FaqItem
            {
                Id = 6,
                SortOrder = 6,
                Question = "I can’t make it to my appointment — what should I do?",
                Answer = "If you’re unable to attend your appointment, please let us know as early as possible. If the appointment is canceled, the deposit will be forfeited. However, the appointment can be rescheduled if you notify us at least 48 hours in advance.",
            }
        );
    }


}
