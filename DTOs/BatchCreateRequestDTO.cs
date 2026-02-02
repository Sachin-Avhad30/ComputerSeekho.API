using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.DTOs
{
    public class BatchCreateRequestDTO
    {
        [Required]
        public string BatchName { get; set; }

        public DateTime? BatchStartDate { get; set; }

        public DateTime? BatchEndDate { get; set; }

        public int? CourseId { get; set; }

        public DateTime? PresentationDate { get; set; }

        public DateTime? FinalPresentationDate { get; set; }

        public decimal? CourseFees { get; set; }

        public DateTime? CourseFeesFrom { get; set; }

        public DateTime? CourseFeesTo { get; set; }

        public bool? BatchIsActive { get; set; }

        public IFormFile? BatchLogo { get; set; }
    }
}
