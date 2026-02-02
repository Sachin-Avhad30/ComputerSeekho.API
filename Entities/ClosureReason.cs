using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerSeekho.API.Entities
{
    [Table("closure_reason_master")]
    public class ClosureReason
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("closure_reason_id")]
        public int ClosureReasonId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("closure_reason_desc")]
        public string ClosureReasonDesc { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;
    }
}
