using ComputerSeekho.API.Entities;

namespace ComputerSeekho.API.Repositories
{
    public interface IStaffRepository
    {
        Task<Staff?> GetByUsernameAsync(string username);
        Task<Staff?> GetByEmailAsync(string email);
        Task<Staff?> GetByIdAsync(int id);
        Task<List<Staff>> GetAllAsync();
        Task<List<Staff>> GetByRoleAsync(string role);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<bool> ExistsByEmailAsync(string email);
        Task<Staff> AddAsync(Staff staff);
        Task<Staff> UpdateAsync(Staff staff);
        Task<bool> DeleteAsync(int id);
    }
}