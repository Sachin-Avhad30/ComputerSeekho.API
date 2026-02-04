using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Entities;

namespace ComputerSeekho.API.Services.Interfaces
{
    public interface IPlacementService
    {
        Task<List<Placement>> GetAllAsync();
        Task CreateAsync(PlacementDto dto);
        Task UpdateAsync(int id, PlacementDto dto);
        Task DeleteAsync(int id);
        Task<List<PlacedStudentPhotoDto>> GetPlacedStudentPhotosAsync(int recruiterId);

        Task<List<BatchPlacementSummaryDto>> GetBatchesWithPlacementsAsync(); // NEW
        Task<List<PlacedStudentDetailDto>> GetPlacedStudentsByBatchAsync(int batchId);

    }
}
