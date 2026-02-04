using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.DTOs
{
    public class StaffSignupRequest
    {
        [Required]
        [MaxLength(100)]
        public string StaffName { get; set; }

        [Required]
        public string StaffMobile { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string StaffEmail { get; set; }

        [Required]
        [MaxLength(100)]
        public string StaffUsername { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(255)]
        public string StaffPassword { get; set; }

        [Required]
        [MaxLength(50)]
        public string StaffRole { get; set; } // "teaching", "admin", "non-teaching"

        [MaxLength(100)]
        public string StaffDesignation { get; set; }

        public string StaffBio { get; set; }

        public IFormFile StaffImage { get; set; }
    }
}
