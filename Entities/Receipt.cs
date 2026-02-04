using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerSeekho.API.Entities
{
    [Table("receipt")]
    public class Receipt
    {
        [Key]
        [Column("receipt_id")]
        public int ReceiptId { get; set; }

        [Required]
        [Column("payment_id")]
        public int PaymentId { get; set; }

        [Required]
        [Column("receipt_amount")]
        [Precision(10, 2)]
        public decimal ReceiptAmount { get; set; }

        [Required]
        [Column("receipt_date")]
        public DateTime ReceiptDate { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        [ForeignKey("PaymentId")]
        public virtual Payment? Payment { get; set; }
    }
}