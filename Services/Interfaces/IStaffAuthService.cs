using ComputerSeekho.API.DTOs.Request;
using ComputerSeekho.API.DTOs.Response;
using Microsoft.AspNetCore.Http;

namespace ComputerSeekho.Application.Services.Interfaces
{
    public interface IStaffAuthService
    {
        Task<StaffJwtResponse> AuthenticateStaffAsync(StaffLoginRequest loginRequest);
        Task<MessageResponse> RegisterStaffWithImageAsync(
            IFormFile? staffImage,
            string staffName,
            string staffMobile,
            string staffEmail,
            string staffUsername,
            string staffPassword,
            string staffRole,
            string? staffDesignation,
            string? staffBio);
        Task<StaffJwtResponse> AuthenticateStaffWithGoogleAsync(string email);
    }
}