namespace ComputerSeekho.API.DTOs
{
    public class ReceiptDTO
    {
        public int ReceiptId { get; set; }
        public int PaymentId { get; set; }
        public decimal ReceiptAmount { get; set; }
        public DateTime ReceiptDate { get; set; }

        // Extended information for PDF generation
        public string StudentName { get; set; } = string.Empty;
        public string StudentMobile { get; set; } = string.Empty;
        public string BatchName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public decimal TotalCourseFees { get; set; }
        public decimal TotalPaidTillNow { get; set; }
        public decimal RemainingBalance { get; set; }
        public string PaymentTypeDesc { get; set; } = string.Empty;
        public string? TransactionReference { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<PreviousPaymentDTO> AllPreviousPayments { get; set; } = new();
    }
}
