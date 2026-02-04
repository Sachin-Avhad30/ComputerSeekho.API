using ComputerSeekho.API.Entities;

namespace ComputerSeekho.API.Repositories.Interfaces
{
    public interface IStaffRepository
    {
        Task<Staff> GetByUsernameAsync(string username);
        Task<Staff> GetByIdAsync(int id);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<bool> ExistsByEmailAsync(string email);
        Task<Staff> CreateAsync(Staff staff);
        Task<Staff> UpdateAsync(Staff staff);
        Task<List<Staff>> GetByStaffRoleAsync(string role);
        Task<List<Staff>> GetAllAsync();
    }
}
