namespace ComputerSeekho.API.DTOs
{
    public class PreviousPaymentDTO
    {
        public int PaymentId { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentTypeDesc { get; set; } = string.Empty;
    }
}
