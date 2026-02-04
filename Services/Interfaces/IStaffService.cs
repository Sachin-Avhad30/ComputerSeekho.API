using ComputerSeekho.API.Entities;

namespace ComputerSeekho.Application.Services.Interfaces
{
    public interface IStaffService
    {
        Task<List<Staff>> GetFacultyListAsync();
        Task<List<Staff>> GetAllStaffAsync();
        Task<Staff> GetStaffByIdAsync(int staffId);
    }
}