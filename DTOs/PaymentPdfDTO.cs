namespace ComputerSeekho.API.DTOs
{
    public class PaymentPdfDTO
    {
        public string StudentName { get; set; }
        public string StudentMobile { get; set; }
        public string StudentAddress { get; set; }
        public string StudentEmail { get; set; }
        public string CourseName { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public decimal ReceiptAmount { get; set; }
        public DateTime ReceiptDate { get; set; }
    }
}