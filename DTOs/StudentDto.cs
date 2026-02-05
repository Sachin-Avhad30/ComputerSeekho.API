using ComputerSeekho.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.DTOs
{
    public class StudentDto
    {
        public int? BatchId { get; set; }
        public int? CourseId { get; set; }
        public string StudentName { get; set; }
        public long StudentMobile { get; set; }
        public string StudentGender { get; set; }
        public DateTime? StudentDob { get; set; }
        public string StudentAddress { get; set; }
        public string StudentQualification { get; set; }

        // USERNAME IS EMAIL
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string StudentUsername { get; set; } // This IS the email!

        public string StudentPassword { get; set; }

        public RegistrationStatus RegistrationStatus { get; set; }
            = RegistrationStatus.PaymentPending;

        public IFormFile? Photo { get; set; }

        // ✅ NEW FIELD: Track source enquiry
        public int? EnquiryId { get; set; }
    }
}