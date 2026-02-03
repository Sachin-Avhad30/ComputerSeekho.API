using Microsoft.AspNetCore.Http;

namespace ComputerSeekho.API.DTOs
{
    public class RecruiterDto
    {
        public string? RecruiterName { get; set; }
        public IFormFile? Logo { get; set; }
        public bool Status { get; set; }
    }

}
