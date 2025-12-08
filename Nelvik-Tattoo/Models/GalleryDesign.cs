namespace Nelvik_Tattoo.Models
{
    public class GalleryDesign
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string? ImagePath { get; set; }
        public string? Description { get; set; }

        // Differensierer mellom “flashes til salgs” og “tidligere motiver”
        public bool IsFlash { get; set; }

        // Kun for flash-design (valgfritt)
        public int? Price { get; set; }
    }

}