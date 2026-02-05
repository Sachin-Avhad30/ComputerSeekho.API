namespace ComputerSeekho.API.DTOs
{
    public class ReceiptDTO
    {
        public int ReceiptId { get; set; }
        public int PaymentId { get; set; }
        public decimal ReceiptAmount { get; set; }
        public DateTime ReceiptDate { get; set; }
        public DateTime CreatedAt { get; set; }

        // Student & Batch Info
        public string StudentName { get; set; } = string.Empty;
        public long StudentMobile { get; set; }
        public string BatchName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;

        // Payment Summary
        public decimal TotalCourseFees { get; set; }
        public decimal TotalPaidTillNow { get; set; }
        public decimal RemainingBalance { get; set; }

        // Current Payment Info
        public string PaymentTypeDesc { get; set; } = string.Empty;
        public string? TransactionReference { get; set; }

        // Previous Payments History
        public List<PreviousPaymentDTO> AllPreviousPayments { get; set; } = new();
    }
}