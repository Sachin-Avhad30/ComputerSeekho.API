using ComputerSeekho.API.Entities;

namespace ComputerSeekho.API.Repositories.Interfaces
{
    public interface IPlacementRepository
    {
        Task<List<Placement>> GetAllAsync();
        Task<Placement?> GetByIdAsync(int id);
        Task AddAsync(Placement placement);
        Task UpdateAsync(Placement placement);
        Task DeleteAsync(Placement placement);
        Task<List<Placement>> GetPlacementsByRecruiterAsync(int recruiterId);
        Task<List<Placement>> GetPlacementsByBatchAsync(int batchId);

    }
}
