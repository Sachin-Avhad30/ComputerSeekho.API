using System.ComponentModel.DataAnnotations;

namespace ComputerSeekho.API.DTOs.Request
{
    public class StaffLoginRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string StaffUsername { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string StaffPassword { get; set; } = string.Empty;
    }
}