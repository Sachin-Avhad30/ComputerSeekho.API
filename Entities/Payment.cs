using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerSeekho.API.Entities
{
    [Table("payment")]
    public class Payment
    {
        [Key]
        [Column("payment_id")]
        public int PaymentId { get; set; }

        [Required]
        [Column("student_id")]
        public int StudentId { get; set; }

        [Required]
        [Column("batch_id")]
        public int BatchId { get; set; }

        [Required]
        [Column("payment_type_id")]
        public int PaymentTypeId { get; set; }

        [Required]
        [Column("payment_amount")]
        [Precision(10, 2)]
        public decimal PaymentAmount { get; set; }

        [Required]
        [Column("payment_date")]
        public DateTime PaymentDate { get; set; }

        [Column("transaction_reference")]
        [MaxLength(100)]
        public string? TransactionReference { get; set; }

        [Column("payment_status")]
        [MaxLength(20)]
        public string PaymentStatus { get; set; } = "COMPLETED"; // PENDING, COMPLETED, FAILED

        [Column("remarks")]
        [MaxLength(500)]
        public string? Remarks { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }

        [ForeignKey("BatchId")]
        public virtual Batch? Batch { get; set; }

        [ForeignKey("PaymentTypeId")]
        public virtual PaymentType? PaymentType { get; set; }

        public virtual ICollection<Receipt>? Receipts { get; set; }
    }
}