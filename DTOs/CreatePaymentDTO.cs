using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.DTOs
{
    public class CreatePaymentDTO
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int BatchId { get; set; }

        [Required]
        public int PaymentTypeId { get; set; }

        [Required]
        [Range(0.01, 999999.99)]
        public decimal PaymentAmount { get; set; }

        [MaxLength(100)]
        public string? TransactionReference { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }
    }
}
