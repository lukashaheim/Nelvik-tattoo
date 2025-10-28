using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nelvik_Tattoo.Models
{
    public class Booking
    {
        public Booking() { }

        public Booking(DateOnly selectedDate, TimeOnly selectedStartTime, TimeOnly selectedEndTime, IFormFile photo)
        {
            SelectedDate = selectedDate;
            SelectedStartTime = selectedStartTime;
            SelectedEndTime = selectedEndTime;
            Photo = photo;
        }

        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Dato")]
        public DateOnly SelectedDate { get; set; }

        [DataType(DataType.Time)]
        [DisplayName("Fra")]
        public TimeOnly SelectedStartTime { get; set; }

        [DataType(DataType.Time)]
        [DisplayName("Til")]
        public TimeOnly SelectedEndTime { get; set; }
        
        [NotMapped, DisplayName("Bilde")]
        public IFormFile? Photo { get; set; }   // lagres ikke i DB

        public string? ImagePath { get; set; } // lagres i DB
        
        public string? Notes { get; set; }

    }
}