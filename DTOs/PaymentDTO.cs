using System;
using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.DTOs
{
   

    // DTO for payment response
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int BatchId { get; set; }
        public string BatchName { get; set; } = string.Empty;
        public int PaymentTypeId { get; set; }
        public string PaymentTypeDesc { get; set; } = string.Empty;
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? TransactionReference { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    
}