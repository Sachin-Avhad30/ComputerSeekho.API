using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerSeekho.API.Entities
{
    [Table("payment_type")]
    public class PaymentType
    {
        [Key]
        [Column("payment_type_id")]
        public int PaymentTypeId { get; set; }

        [Required]
        [Column("payment_type_desc")]
        [MaxLength(100)]
        public string PaymentTypeDesc { get; set; } = string.Empty;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public virtual ICollection<Payment>? Payments { get; set; }
    }
}