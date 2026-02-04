using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.Entities
{
    [Table("staff_master")]
    public class Staff
    {
        [Key]
        [Column("staff_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StaffId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("staff_name")]
        public string StaffName { get; set; }

        [MaxLength(255)]
        [Column("photo_url")]
        public string PhotoUrl { get; set; }

        [Required]
        [Column("staff_mobile")]
        public string StaffMobile { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        [Column("staff_email")]
        public string StaffEmail { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("staff_username")]
        public string StaffUsername { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("staff_password")]
        public string StaffPassword { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("staff_role")]
        public string StaffRole { get; set; } // "teaching", "admin", "non-teaching"

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("staff_bio", TypeName = "TEXT")]
        public string StaffBio { get; set; }

        [MaxLength(100)]
        [Column("staff_designation")]
        public string StaffDesignation { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}
