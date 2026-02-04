namespace ComputerSeekho.API.DTOs
{
    public class AnnouncementResponseDTO
    {
        public int AnnouncementId { get; set; }
        public string AnnouncementText { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
