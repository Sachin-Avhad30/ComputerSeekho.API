namespace ComputerSeekho.API.DTOs
{
    public class InstallmentCalculationDTO
    {
        public int StudentId { get; set; }
        public int BatchId { get; set; }
        public decimal TotalCourseFees { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal RemainingBalance { get; set; }
        public int InstallmentsPaid { get; set; }
        public List<PreviousPaymentDTO> PreviousPayments { get; set; } = new();
    }
}
