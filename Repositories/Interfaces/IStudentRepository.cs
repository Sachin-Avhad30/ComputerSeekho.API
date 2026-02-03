using ComputerSeekho.API.Entities;

namespace ComputerSeekho.API.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<List<StudentMaster>> GetAllAsync();
        Task<StudentMaster> GetByIdAsync(int id);
        Task AddAsync(StudentMaster student);
        Task UpdateAsync(StudentMaster student);
        Task DeleteAsync(StudentMaster student);
    }
}
