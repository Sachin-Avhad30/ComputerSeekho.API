using ComputerSeekho.API.Entities;

namespace ComputerSeekho.API.Repositories.Interfaces
{
    public interface IAnnouncementRepository
    {
        Task<Announcement> GetByIdAsync(int id);
        Task<IEnumerable<Announcement>> GetAllAsync();
        Task<IEnumerable<string>> GetActiveAnnouncementTextsAsync(DateTime currentDateTime);
        Task<Announcement> AddAsync(Announcement announcement);
        Task<Announcement> UpdateAsync(Announcement announcement);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
