using System.ComponentModel.DataAnnotations;

namespace Nelvik_Tattoo.Models
{
    public class AboutPage
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        // Path or URL to the image used on the about page
        [StringLength(500)]
        public string ImagePath { get; set; }
    }
}