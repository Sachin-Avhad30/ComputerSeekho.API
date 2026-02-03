using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.Entities
{
    [Table("course_master")]
    public class Course : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("course_id")]
        public int CourseId { get; set; }

        [Required]
        [Column("course_name")]
        [MaxLength(100)]
        public string CourseName { get; set; }

        [Column("course_description", TypeName = "TEXT")]
        public string? CourseDescription { get; set; }

        [Column("course_duration")]
        public int? CourseDuration { get; set; }

        [Required]
        [Column("course_fees", TypeName = "decimal(10,2)")]
        public decimal CourseFees { get; set; }

        [Column("course_fees_from")]
        public DateTime? CourseFeesFrom { get; set; }

        [Column("course_fees_to")]
        public DateTime? CourseFeesTo { get; set; }

        [Column("course_syllabus", TypeName = "TEXT")]
        public string? CourseSyllabus { get; set; }

        [Column("age_grp_type")]
        [MaxLength(20)]
        public string? AgeGrpType { get; set; }

        [Column("course_is_active")]
        public bool CourseIsActive { get; set; } = true;

        [Column("cover_photo")]
        [MaxLength(255)]
        public string? CoverPhoto { get; set; }

        [Column("video_id")]
        public int? VideoId { get; set; }

        //[Column("created_at")]
        //public DateTime CreatedAt { get; set; }

        //[Column("updated_at")]
        //public DateTime UpdatedAt { get; set; }
    }
}
