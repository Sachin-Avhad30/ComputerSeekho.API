using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerSeekho.API.Entities
{
    [Table("enquiry_master")]
    public class Enquiry : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("enquiry_id")]
        public int EnquiryId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("enquirer_name")]
        public string EnquirerName { get; set; } = string.Empty;

        [MaxLength(100)]
        [Column("student_name")]
        public string? StudentName { get; set; }

        [MaxLength(200)]
        [Column("enquirer_address")]
        public string? EnquirerAddress { get; set; }

        [Required]
        [Column("enquirer_mobile")]
        public long EnquirerMobile { get; set; }

        [Column("enquirer_alternate_mobile")]
        public long? EnquirerAlternateMobile { get; set; }

        [MaxLength(100)]
        [Column("enquirer_email_id")]
        public string? EnquirerEmailId { get; set; }

        [Column("enquiry_date")]
        public DateTime EnquiryDate { get; set; } = DateTime.Now;

        [Column("enquirer_query", TypeName = "TEXT")]
        public string? EnquirerQuery { get; set; }

        // Foreign Key: Closure Reason
        [Column("closure_reason_id")]
        public int? ClosureReasonId { get; set; }

        [ForeignKey("ClosureReasonId")]
        public ClosureReason? ClosureReason { get; set; }

        [MaxLength(255)]
        [Column("closure_reason")]
        public string? ClosureReasonText { get; set; }

        [Column("enquiry_processed_flag")]
        public bool EnquiryProcessedFlag { get; set; } = false;

        // Foreign Key: Course
        [Column("course_id")]
        public int? CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }

        // Foreign Key: Staff
        [Column("staff_id")]
        public int? StaffId { get; set; }

        [ForeignKey("StaffId")]
        public Staff? Staff { get; set; }

        [Column("inquiry_counter")]
        public int InquiryCounter { get; set; } = 0;

        [Column("followup_date")]
        public DateTime? FollowupDate { get; set; }

        [MaxLength(20)]
        [Column("enquiry_source")]
        public string? EnquirySource { get; set; } // Telephonic / Walk-in / Email / Online / Fax

        [Column("special_instructions", TypeName = "TEXT")]
        public string? SpecialInstructions { get; set; }

        [Column("is_closed")]
        public bool IsClosed { get; set; } = false;
    }
}