using Nelvik_Tattoo.Models;

namespace Nelvik_Tattoo.Data;

public static class ApplicationDbInitializer
{
    public static void Initialize(ApplicationDbContext db)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        
        Console.WriteLine(">>> Har GalleryDesigns: " + db.GalleryDesigns.Count());


        db.GalleryDesigns.AddRange(
            new GalleryDesign
            {
                Title = "Test Flash 1",
                ImagePath = "image0.jpeg", // Ligger i wwwroot/Images/flash/
                IsFlash = true,
                Price = 1200,
                Description = "Et kult flashmotiv med tribal-design"
            },
            new GalleryDesign
            {
                Title = "Test Flash 2",
                ImagePath = "image1.jpeg",
                IsFlash = true,
                Price = 1500,
                Description = "En annen variant av flashmotiv"
            },
            new GalleryDesign
            {
                Title = "Test Flash 3",
                ImagePath = "image2.jpeg", // Ligger i wwwroot/Images/flash/
                IsFlash = true,
                Price = 1200,
                Description = "Et kult flashmotiv med tribal-design"
            },
            new GalleryDesign
            {
                Title = "Test Flash 4",
                ImagePath = "image3.jpeg", // Ligger i wwwroot/Images/flash/
                IsFlash = true,
                Price = 1200,
                Description = "Et kult flashmotiv med tribal-design"
            },
            new GalleryDesign
            {
                Title = "Test Flash 5",
                ImagePath = "image4.jpeg", // Ligger i wwwroot/Images/flash/
                IsFlash = true,
                Price = 1200,
                Description = "Et kult flashmotiv med tribal-design"
            },
            new GalleryDesign
            {
                Title = "Test Flash 1",
                ImagePath = "image0.jpeg", // Ligger i wwwroot/Images/flash/
                IsFlash = true,
                Price = 1200,
                Description = "Et kult flashmotiv med tribal-design"
            },
            new GalleryDesign
            {
                Title = "Test Flash 2",
                ImagePath = "image1.jpeg",
                IsFlash = true,
                Price = 1500,
                Description = "En annen variant av flashmotiv"
            },
            new GalleryDesign
            {
                Title = "Test Flash 3",
                ImagePath = "image2.jpeg", // Ligger i wwwroot/Images/flash/
                IsFlash = true,
                Price = 1200,
                Description = "Et kult flashmotiv med tribal-design"
            },
            new GalleryDesign
            {
                Title = "Test Flash 4",
                ImagePath = "image3.jpeg", // Ligger i wwwroot/Images/flash/
                IsFlash = true,
                Price = 1200,
                Description = "Et kult flashmotiv med tribal-design"
            },
            new GalleryDesign
            {
                Title = "Test Flash 5",
                ImagePath = "image4.jpeg", // Ligger i wwwroot/Images/flash/
                IsFlash = true,
                Price = 1200,
                Description = "Et kult flashmotiv med tribal-design"
            },
            new GalleryDesign
            {
                Title = "Ferdig motiv 1",
                ImagePath = "image00001.jpeg", // Ligger i wwwroot/Images/Gallery/
                IsFlash = false,
                Description = "Et tidligere motiv som er ferdigstilt"
            },
            new GalleryDesign
            {
                Title = "Ferdig motiv 2",
                ImagePath = "image00002_1.jpeg", // Ligger i wwwroot/Images/Gallery/
                IsFlash = false,
                Description = "Et tidligere motiv som er ferdigstilt"
            },
            new GalleryDesign
            {
                Title = "Ferdig motiv 3",
                ImagePath = "image00005.jpeg", // Ligger i wwwroot/Images/Gallery/
                IsFlash = false,
                Description = "Et tidligere motiv som er ferdigstilt"
            }
            
            
        );


        db.SaveChanges();
    }
}