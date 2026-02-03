using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.DTOs
{
    public class EmailRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
