using Nelvik_Tattoo.Models;

namespace Nelvik_Tattoo.Data
{
    public static class ApplicationDbInitializer
    {
        public static void Initialize(ApplicationDbContext db, IWebHostEnvironment env)
        {
            // 1. Slett databasen (fordi du ønsker ren start hver gang)
            db.Database.EnsureDeleted();

            // 2. Opprett databasen
            db.Database.EnsureCreated();
            Console.WriteLine("Database created. Antall motiver: " + db.GalleryDesigns.Count());

            // 3. Seed data KUN hvis databasen er tom
            if (!db.GalleryDesigns.Any())
            {
                Console.WriteLine("Seeding default gallery designs...");

                var flashDir = Path.Combine(env.WebRootPath, "Images/Preloaded/Flash");
                var finishedDir = Path.Combine(env.WebRootPath, "Images/Preloaded/Finished");

                // ---- SEED FLASH ----
                if (Directory.Exists(flashDir))
                {
                    foreach (var file in Directory.GetFiles(flashDir))
                    {
                        db.GalleryDesigns.Add(new GalleryDesign
                        {
                            Title = Path.GetFileNameWithoutExtension(file),
                            ImagePath = "/Images/Preloaded/Flash/" + Path.GetFileName(file),
                            IsFlash = true,
                            Price = 1000
                        });
                    }
                }

                // ---- SEED FINISHED ----
                if (Directory.Exists(finishedDir))
                {
                    foreach (var file in Directory.GetFiles(finishedDir))
                    {
                        db.GalleryDesigns.Add(new GalleryDesign
                        {
                            Title = Path.GetFileNameWithoutExtension(file),
                            ImagePath = "/Images/Preloaded/Finished/" + Path.GetFileName(file),
                            IsFlash = false
                        });
                    }
                }

                db.SaveChanges();
                Console.WriteLine("Seeding complete.");
            }
        }
    }
}
