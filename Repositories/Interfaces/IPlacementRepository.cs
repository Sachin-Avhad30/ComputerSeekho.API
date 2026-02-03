using ComputerSeekho.API.Entities;

namespace ComputerSeekho.API.Repositories.Interfaces
{
    public interface IPlacementRepository
    {
        Task<List<PlacementMaster>> GetAllAsync();
        Task<PlacementMaster?> GetByIdAsync(int id);
        Task AddAsync(PlacementMaster placement);
        Task UpdateAsync(PlacementMaster placement);
        Task DeleteAsync(PlacementMaster placement);
        Task<List<PlacementMaster>> GetPlacementsByRecruiterAsync(int recruiterId);

    }
}
