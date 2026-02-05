namespace ComputerSeekho.API.DTOs
{
    public class PaymentPdfDTO
    {
        public string StudentName { get; set; } = string.Empty;
        public string StudentMobile { get; set; } = string.Empty;
        public string StudentAddress { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string BatchName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; } = string.Empty;
        public decimal ReceiptAmount { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int ReceiptId { get; set; }

        // ✅ NEW FIELDS: Payment Summary
        public decimal TotalCourseFees { get; set; }
        public decimal TotalPaidTillNow { get; set; }
        public decimal RemainingBalance { get; set; }
        public string? TransactionReference { get; set; }

        // ✅ NEW: All previous payments including this one
        public List<PreviousPaymentDTO> AllPreviousPayments { get; set; } = new();
    }
}