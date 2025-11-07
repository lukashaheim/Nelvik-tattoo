using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nelvik_Tattoo.Models
{
    public class Booking
    {
        public Booking() { }

        public Booking(
            string design,
            string placement,
            int size,
            string budget,
            string email,
            string phone,
            int preferredCM,
            string comment,
            DateOnly selectedDate, 
            TimeOnly selectedStartTime, 
            TimeOnly selectedEndTime, 
            IFormFile photo
            
            )
        {
            Design = design;
            Placement = placement;
            Size = size;
            Budget = budget;
            Email = email;
            Phone = phone;
            PrefferedCM = preferredCM;
            Comment = comment;
            SelectedDate = selectedDate;
            SelectedStartTime = selectedStartTime;
            SelectedEndTime = selectedEndTime;
            Photo = photo;
        }

        public int Id { get; set; }
        
        public string Design { get; set; }
        
        public string Placement { get; set; }
        
        public int Size { get; set; }
        
        public string Budget { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        
        [Phone]
        public string Phone { get; set; }
        
        public int PrefferedCM { get; set; }
        
        public string Comment { get; set; }

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