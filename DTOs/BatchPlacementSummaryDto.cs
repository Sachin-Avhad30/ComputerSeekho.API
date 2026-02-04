namespace ComputerSeekho.API.DTOs
{
    public class BatchPlacementSummaryDto
    {
        public int BatchId { get; set; }
        public string BatchName { get; set; }
        public string? BatchLogo { get; set; }
        public int PlacedStudentCount { get; set; }
    }
}
