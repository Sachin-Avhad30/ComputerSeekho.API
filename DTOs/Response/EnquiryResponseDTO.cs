namespace ComputerSeekho.API.DTOs.Response
{
    public class EnquiryResponseDTO
    {
        public int EnquiryId { get; set; }
        public string EnquirerName { get; set; } = string.Empty;
        public long EnquirerMobile { get; set; }
        public string CourseName { get; set; } = "N/A";
        public DateTime? FollowupDate { get; set; }
        public bool IsClosed { get; set; }
    }
}