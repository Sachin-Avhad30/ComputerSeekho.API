using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Entities;

namespace ComputerSeekho.API.Services.Interfaces
{
    public interface IBatchService
    {
        Task<BatchResponseDTO> CreateBatchWithImageAsync(BatchCreateRequestDTO dto);
        Task<BatchResponseDTO> UpdateBatchWithImageAsync(int batchId, BatchUpdateRequestDTO dto);
        Task<IEnumerable<BatchResponseDTO>> GetAllBatchesAsync();
        Task<IEnumerable<BatchResponseDTO>> GetActiveBatchesAsync();
        Task<IEnumerable<BatchResponseDTO>> GetBatchesByCourseIdAsync(int courseId);
        Task<IEnumerable<BatchResponseDTO>> SearchBatchesAsync(string keyword);
        Task<BatchResponseDTO> GetBatchByIdAsync(int batchId);
        Task<Batch> GetBatchEntityByIdAsync(int batchId);
        Task<bool> DeleteBatchAsync(int batchId);

        Task<string?> GetBatchNameByIdAsync(int batchId);

    }
}
