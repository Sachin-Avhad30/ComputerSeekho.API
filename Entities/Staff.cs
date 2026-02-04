using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerSeekho.API.Entities
{
    [Table("staff_master")]
    public class Staff : BaseEntity
    {
        [Key]
        [Column("staff_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StaffId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("staff_name")]
        public string StaffName { get; set; } = string.Empty;

        [StringLength(255)]
        [Column("photo_url")]
        public string? PhotoUrl { get; set; }

        [Required]
        [Column("staff_mobile")]
        public string StaffMobile { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        [Column("staff_email")]
        public string StaffEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column("staff_username")]
        public string StaffUsername { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column("staff_password")]
        public string StaffPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Column("staff_role")]
        public string StaffRole { get; set; } = string.Empty; // "teaching" or "non-teaching"

        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("staff_bio", TypeName = "TEXT")]
        public string? StaffBio { get; set; }

        [StringLength(100)]
        [Column("staff_designation")]
        public string? StaffDesignation { get; set; }
    }
}