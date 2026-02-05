using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ComputerSeekho.API.Enums;

namespace ComputerSeekho.API.Entities
{
    [Table("student_master")]
    public class Student : BaseEntity
    {
        // =========================
        // PRIMARY KEY
        // =========================
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("student_id")]
        public int StudentId { get; set; }

        // =========================
        // FOREIGN KEYS
        // =========================
        [Column("batch_id")]
        public int? BatchId { get; set; }

        [ForeignKey("BatchId")]
        public virtual Batch? Batch { get; set; }

        [Column("course_id")]
        public int? CourseId { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }

        // =========================
        // STUDENT DETAILS
        // =========================
        [Required]
        [MaxLength(100)]
        [Column("student_name")]
        public string StudentName { get; set; }

        [Required]
        [Column("student_mobile")]
        public long StudentMobile { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("student_gender")]
        public string StudentGender { get; set; }

        [Column("student_dob")]
        public DateTime? StudentDob { get; set; }

        [MaxLength(200)]
        [Column("student_address")]
        public string StudentAddress { get; set; }

        [MaxLength(50)]
        [Column("student_qualification")]
        public string StudentQualification { get; set; }

        // =========================
        // AUTH (USERNAME IS EMAIL)
        // =========================
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        [Column("student_username")]
        public string StudentUsername { get; set; } // This IS the email!


        [MaxLength(255)]
        [Column("student_password")]
        public string StudentPassword { get; set; }

        // =========================
        // PHOTO
        // =========================
        [MaxLength(255)]
        [Column("photo_url")]
        public string PhotoUrl { get; set; }

        // =========================
        // STATUS
        // =========================
        [Column("registration_status")]
        public RegistrationStatus RegistrationStatus { get; set; }
            = RegistrationStatus.PaymentPending;

        // =========================
        // NAVIGATION PROPERTIES
        // =========================

        /// <summary>
        /// Collection of all payments made by this student
        /// </summary>
        public virtual ICollection<Payment>? Payments { get; set; }
    }
}