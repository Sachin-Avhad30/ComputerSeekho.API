using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Entities;

namespace ComputerSeekho.API.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentMaster>> GetAllAsync();
        Task CreateAsync(StudentDto dto);
        Task UpdateAsync(int id, StudentDto dto);
        Task DeleteAsync(int id);
    }
}
