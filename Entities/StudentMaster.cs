using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ComputerSeekho.API.Enum;

namespace ComputerSeekho.API.Entities
{
        [Table("student_master")]
        public class StudentMaster
        {
            // =========================
            // PRIMARY KEY
            // =========================

            [Key]
            [Column("student_id")]
            public int StudentId { get; set; }

            // =========================
            // FOREIGN KEYS
            // =========================

            [Column("batch_id")]
            public int? BatchId { get; set; }

            [Column("course_id")]
            public int? CourseId { get; set; }

            // =========================
            // STUDENT DETAILS
            // =========================

            [Required]
            [Column("student_name")]
            [MaxLength(100)]
            public string StudentName { get; set; }

            [Required]
            [Column("student_mobile")]
            public long StudentMobile { get; set; }

            [Column("student_gender")]
            [MaxLength(10)]
            public string StudentGender { get; set; }

            [Column("student_dob")]
            public DateTime? StudentDob { get; set; }

            [Column("student_address")]
            [MaxLength(200)]
            public string StudentAddress { get; set; }

            [Column("student_qualification")]
            [MaxLength(50)]
            public string StudentQualification { get; set; }

            // =========================
            // AUTHENTICATION
            // =========================

            [Column("student_username")]
            [MaxLength(100)]
            public string StudentUsername { get; set; }

            [Column("student_password")]
            [MaxLength(255)]
            public string StudentPassword { get; set; }

            // =========================
            // PHOTO
            // =========================

            [Column("photo_url")]
            [MaxLength(255)]
            public string PhotoUrl { get; set; }

            // =========================
            // REGISTRATION STATUS (ENUM)
            // =========================

            [Column("registration_status")]
            public RegistrationStatus RegistrationStatus { get; set; }
                = RegistrationStatus.PaymentPending;

            // =========================
            // AUDIT FIELDS
            // =========================

            [Column("created_at")]
            public DateTime? CreatedAt { get; set; }

            [Column("updated_at")]
            public DateTime? UpdatedAt { get; set; }
        }
}
