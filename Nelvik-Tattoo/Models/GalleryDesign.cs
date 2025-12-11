namespace Nelvik_Tattoo.Models
{
    public class GalleryDesign
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string? ImagePath { get; set; }
        public string? Description { get; set; }
        public string? Placement { get; set; }

        // Differensierer mellom “flashes til salgs” og “tidligere motiver”
        public bool IsFlash { get; set; }
        public bool IsFinished { get; set; }


        // Kun for flash-design (valgfritt)
        public int? Price { get; set; }
        
        public bool IsFeatured { get; set; }

    }

}