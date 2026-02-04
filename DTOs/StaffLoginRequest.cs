using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.DTOs
{
    public class StaffLoginRequest
    {
        [Required]
        public string StaffUsername { get; set; }

        [Required]
        public string StaffPassword { get; set; }
    }
}
