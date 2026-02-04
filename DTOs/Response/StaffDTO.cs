namespace ComputerSeekho.API.DTOs.Response
{
    public class StaffDTO
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string StaffMobile { get; set; } = string.Empty;
        public string StaffEmail { get; set; } = string.Empty;
        public string StaffRole { get; set; } = string.Empty;
        public string? StaffBio { get; set; }
        public string? StaffDesignation { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}