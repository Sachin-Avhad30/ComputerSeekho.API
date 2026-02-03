using ComputerSeekho.API.DTOs;

namespace ComputerSeekho.API.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEnquiryEmailAsync(EmailRequestDTO dto);
    }
}
