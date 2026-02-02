using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.Entities
{
    [Table("batch_master")]
    public class Batch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("batch_id")]
        public int BatchId { get; set; }

        [Required]
        [Column("batch_name")]
        [MaxLength(100)]
        public string BatchName { get; set; }

        [Column("batch_start_date")]
        public DateTime? BatchStartDate { get; set; }

        [Column("batch_end_date")]
        public DateTime? BatchEndDate { get; set; }

        [Required]
        [Column("course_id")]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }

        [Column("presentation_date")]
        public DateTime? PresentationDate { get; set; }

        [Column("course_fees", TypeName = "decimal(10,2)")]
        public decimal? CourseFees { get; set; }

        [Column("course_fees_from")]
        public DateTime? CourseFeesFrom { get; set; }

        [Column("course_fees_to")]
        public DateTime? CourseFeesTo { get; set; }

        [Column("batch_is_active")]
        public bool BatchIsActive { get; set; } = true;

        [Column("final_presentation_date")]
        public DateTime? FinalPresentationDate { get; set; }

        [Column("batch_logo_url")]
        [MaxLength(255)]
        public string? BatchLogoUrl { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
