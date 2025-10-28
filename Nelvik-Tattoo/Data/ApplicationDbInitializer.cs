namespace Nelvik_Tattoo.Data;

public class ApplicationDbInitializer
{
    public static void Initialize(ApplicationDbContext db)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }
}