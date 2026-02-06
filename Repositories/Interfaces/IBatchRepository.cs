using ComputerSeekho.API.Entities;

namespace ComputerSeekho.API.Repositories.Interfaces
{
    public interface IBatchRepository
    {
        Task<Batch> GetByIdAsync(int id);
        Task<IEnumerable<Batch>> GetAllAsync();
        Task<IEnumerable<Batch>> GetActiveBatchesAsync();
        Task<IEnumerable<Batch>> GetBatchesByCourseIdAsync(int courseId);
        Task<IEnumerable<Batch>> SearchBatchesAsync(string keyword);
        Task<Batch> AddAsync(Batch batch);
        Task<Batch> UpdateAsync(Batch batch);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<string?> GetBatchNameByIdAsync(int batchId);

    }
}
