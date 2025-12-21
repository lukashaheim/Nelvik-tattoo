namespace Nelvik_Tattoo.Models
{
    public class GalleryDesignImage
    {
        public int Id { get; set; }

        public int GalleryDesignId { get; set; }
        public GalleryDesign GalleryDesign { get; set; } = null!;

        public string ImagePath { get; set; } = "";
        public int SortOrder { get; set; } = 0;
    }
}