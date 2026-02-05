using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.DTOs.Request
{
    public class EnquiryUpdateRequestDTO
    {
        [MaxLength(100)]
        public string? EnquirerName { get; set; }

        [MaxLength(100)]
        public string? StudentName { get; set; }

        [MaxLength(200)]
        public string? EnquirerAddress { get; set; }

        public long? EnquirerMobile { get; set; }

        public long? EnquirerAlternateMobile { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? EnquirerEmailId { get; set; }

        public string? EnquirerQuery { get; set; }

        [MaxLength(20)]
        public string? EnquirySource { get; set; } // Telephonic / Walk-in / Email / Online / Fax

        public int? CourseId { get; set; } // Course can be changed

        public DateTime? FollowupDate { get; set; } // Can update follow-up date

        public string? SpecialInstructions { get; set; }
    }
}