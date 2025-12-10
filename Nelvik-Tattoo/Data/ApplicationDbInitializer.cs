using Nelvik_Tattoo.Models;

namespace Nelvik_Tattoo.Data
{
    public static class ApplicationDbInitializer
    {
        public static void Initialize(ApplicationDbContext db, IWebHostEnvironment env)
        {
            // Slett alltid databasen (fordi dere ønsker ren start hver gang)
            //db.Database.EnsureDeleted();

            // Opprett tabeller (Identity, Gallery, FAQ, Booking, osv.)
            db.Database.EnsureCreated();

            Console.WriteLine(">>> Database opprettet. Antall motiver nå: " + db.GalleryDesigns.Count());


            // ----------------------------
            // SEED PRELOADED GALLERY FILES
            // ----------------------------

            var flashDir = Path.Combine(env.WebRootPath, "Images/Preloaded/Flash");
            var finishedDir = Path.Combine(env.WebRootPath, "Images/Preloaded/Finished");

            if (Directory.Exists(flashDir))
            {
                foreach (var file in Directory.GetFiles(flashDir))
                {
                    db.GalleryDesigns.Add(new GalleryDesign
                    {
                        Title = Path.GetFileNameWithoutExtension(file),
                        ImagePath = "/Images/Preloaded/Flash/" + Path.GetFileName(file),
                        IsFlash = true,
                        Price = 1000,
                        Description = "Forhåndslastet flashmotiv"
                    });
                }
            }

            if (Directory.Exists(finishedDir))
            {
                foreach (var file in Directory.GetFiles(finishedDir))
                {
                    db.GalleryDesigns.Add(new GalleryDesign
                    {
                        Title = Path.GetFileNameWithoutExtension(file),
                        ImagePath = "/Images/Preloaded/Finished/" + Path.GetFileName(file),
                        IsFlash = false,
                        Description = "Forhåndslastet tidligere motiv"
                    });
                }
            }

            db.SaveChanges();

            Console.WriteLine(">>> Seeding ferdig. Motiver i databasen: " + db.GalleryDesigns.Count());
        }
    }
}
