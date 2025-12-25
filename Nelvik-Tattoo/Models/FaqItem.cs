using System;
using System.ComponentModel.DataAnnotations;

namespace Nelvik_Tattoo.Models
{
    public class FaqItem
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Question { get; set; } = string.Empty;

        [Required]
        public string Answer { get; set; } = string.Empty;

        // Valgfritt: kontroll på rekkefølge i visning
        public int SortOrder { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}