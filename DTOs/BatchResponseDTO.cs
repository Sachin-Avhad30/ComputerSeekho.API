namespace ComputerSeekho.API.DTOs
{
    public class BatchResponseDTO
    {
        public int BatchId { get; set; }
        public string BatchName { get; set; }
        public DateTime? BatchStartDate { get; set; }
        public DateTime? BatchEndDate { get; set; }
        public int? CourseId { get; set; }
        public string? CourseName { get; set; }
        public DateTime? PresentationDate { get; set; }
        public decimal? CourseFees { get; set; }
        public DateTime? CourseFeesFrom { get; set; }
        public DateTime? CourseFeesTo { get; set; }
        public bool BatchIsActive { get; set; }
        public DateTime? FinalPresentationDate { get; set; }
        public string? BatchLogoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
