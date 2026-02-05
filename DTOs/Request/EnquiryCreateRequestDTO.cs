using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.DTOs.Request
{
    public class EnquiryCreateRequestDTO
    {
        [Required(ErrorMessage = "Enquirer name is required")]
        [MaxLength(100)]
        public string EnquirerName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? StudentName { get; set; }

        [MaxLength(200)]
        public string? EnquirerAddress { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        public long EnquirerMobile { get; set; }

        public long? EnquirerAlternateMobile { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? EnquirerEmailId { get; set; }

        public string? EnquirerQuery { get; set; }

        [MaxLength(20)]
        public string? EnquirySource { get; set; } // Telephonic / Walk-in / Email / Online / Fax

        public int? CourseId { get; set; }

        public int? StaffId { get; set; }

        public DateTime? FollowupDate { get; set; } // Optional (default +3 days)

        public string? SpecialInstructions { get; set; }
    }
}