using ComputerSeekho.API.DTOs;

namespace ComputerSeekho.API.Services.Interfaces
{
    public interface IStaffAuthService
    {
        Task<StaffJwtResponse> AuthenticateStaffAsync(StaffLoginRequest loginRequest);
        Task<MessageResponse> RegisterStaffWithImageAsync(StaffSignupRequest signupRequest);
        Task<List<StaffInfoResponse>> GetFacultyListAsync();
        Task<List<StaffInfoResponse>> GetAllStaffAsync();
        Task<StaffInfoResponse> GetStaffByIdAsync(int staffId);
    }
}
