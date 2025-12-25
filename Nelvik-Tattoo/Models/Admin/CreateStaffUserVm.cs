using System.ComponentModel.DataAnnotations;

namespace Nelvik_Tattoo.Models.Admin
{
    public class CreateStaffUserVm
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-post")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Passord må være minst {1} tegn.")]
        [Display(Name = "Passord")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passordene er ikke like.")]
        [Display(Name = "Bekreft passord")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}