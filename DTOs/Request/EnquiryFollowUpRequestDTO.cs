using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.DTOs.Request
{
    public class EnquiryFollowUpRequestDTO
    {
        [Required(ErrorMessage = "Enquiry ID is required")]
        public int EnquiryId { get; set; }

        public string? Remarks { get; set; }

        public string? SpecialInstructions { get; set; }

        public DateTime? NextFollowupDate { get; set; } // default today + 3

        public bool? CloseEnquiry { get; set; }

        public int? ClosureReasonId { get; set; } // optional

        public string? ClosureReasonText { get; set; } // if "Other"
    }
}