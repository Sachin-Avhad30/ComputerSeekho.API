namespace ComputerSeekho.API.DTOs
{
    public class StudentPaymentSummaryDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int BatchId { get; set; }
        public string BatchName { get; set; } = string.Empty;
        public decimal TotalCourseFees { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal RemainingBalance { get; set; }
        public int InstallmentsPaid { get; set; }
        public List<PaymentDTO> PaymentHistory { get; set; } = new();
    }
}
