using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.Entities
{
    [Table("announcement_master")]
    public class Announcement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("announcement_id")]
        public int AnnouncementId { get; set; }

        [Required]
        [Column("announcement_text")]
        public string AnnouncementText { get; set; }

        [Required]
        [Column("valid_from")]
        public DateTime ValidFrom { get; set; }

        [Required]
        [Column("valid_to")]
        public DateTime ValidTo { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
