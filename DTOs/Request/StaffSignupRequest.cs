using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.DTOs.Request
{
    public class StaffSignupRequest
    {
        [Required(ErrorMessage = "Staff name is required")]
        [StringLength(100, ErrorMessage = "Staff name must be less than 100 characters")]
        public string StaffName { get; set; } = string.Empty;

        public IFormFile? StaffImage { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        public string StaffMobile { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email should be valid")]
        [StringLength(100)]
        public string StaffEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters")]
        public string StaffUsername { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 40 characters")]
        public string StaffPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Staff role is required")]
        public string StaffRole { get; set; } = string.Empty; // "admin" or "teaching"

        public string? StaffBio { get; set; }

        public string? StaffDesignation { get; set; }
    }
}