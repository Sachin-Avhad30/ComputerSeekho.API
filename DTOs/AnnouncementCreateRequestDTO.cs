using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.DTOs
{
    public class AnnouncementCreateRequestDTO
    {
        [Required]
        public string AnnouncementText { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public bool? IsActive { get; set; }
    }
}
