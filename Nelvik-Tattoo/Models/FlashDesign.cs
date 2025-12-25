using System.ComponentModel.DataAnnotations;

namespace Nelvik_Tattoo.Models
{
    public class FlashDesign
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Range(0, 100000)]
        public decimal Price { get; set; }

        // Hvor bildet ligger i wwwroot
        public string ImagePath { get; set; } = string.Empty;

        // Om motivet er tilgjengelig for booking eller solgt
        public bool IsAvailable { get; set; } = true;
    }
}