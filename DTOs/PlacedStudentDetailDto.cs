namespace ComputerSeekho.API.DTOs
{
    public class PlacedStudentDetailDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string CompanyName { get; set; }
        public string? CompanyLogo { get; set; }
        public int BatchId { get; set; }
        public string BatchName { get; set; }
    }
}
