using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Enum;
using ComputerSeekho.API.Repositories.Interfaces;
using ComputerSeekho.API.Services.Interfaces;
using ComputerSeekho.Application.Services.Interfaces;

namespace ComputerSeekho.API.Services
{
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _batchRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IFileStorageService _fileStorageService;

        public BatchService(
            IBatchRepository batchRepository,
            ICourseRepository courseRepository,
            IFileStorageService fileStorageService)
        {
            _batchRepository = batchRepository;
            _courseRepository = courseRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<BatchResponseDTO> CreateBatchWithImageAsync(BatchCreateRequestDTO dto)
        {
            try
            {
                // Validate course exists if courseId is provided
                if (dto.CourseId.HasValue)
                {
                    var courseExists = await _courseRepository.ExistsAsync(dto.CourseId.Value);
                    if (!courseExists)
                    {
                        throw new KeyNotFoundException($"Course with ID {dto.CourseId.Value} not found");
                    }
                }

                // Map DTO to Entity
                var batch = new Batch
                {
                    BatchName = dto.BatchName,
                    BatchStartDate = dto.BatchStartDate,
                    BatchEndDate = dto.BatchEndDate,
                    CourseId = dto.CourseId ?? 0, // Required field
                    PresentationDate = dto.PresentationDate,
                    FinalPresentationDate = dto.FinalPresentationDate,
                    CourseFees = dto.CourseFees,
                    CourseFeesFrom = dto.CourseFeesFrom,
                    CourseFeesTo = dto.CourseFeesTo,
                    BatchIsActive = dto.BatchIsActive ?? true
                };

                // Handle logo upload
                var logoUrl = await _fileStorageService.StoreImageAsync(dto.BatchLogo,UploadType.Batch,dto.BatchName);

                batch.BatchLogoUrl = logoUrl;

                // Save to database
                var savedBatch = await _batchRepository.AddAsync(batch);

                // Map to response DTO
                return MapToDTO(savedBatch);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating batch: {ex.Message}", ex);
            }
        }

        public async Task<BatchResponseDTO> UpdateBatchWithImageAsync(int batchId, BatchUpdateRequestDTO dto)
        {
            try
            {
                // Get existing batch
                var batch = await _batchRepository.GetByIdAsync(batchId);
                if (batch == null)
                {
                    throw new KeyNotFoundException($"Batch with ID {batchId} not found");
                }

                // Validate course exists if courseId is provided
                if (dto.CourseId.HasValue)
                {
                    var courseExists = await _courseRepository.ExistsAsync(dto.CourseId.Value);
                    if (!courseExists)
                    {
                        throw new KeyNotFoundException($"Course with ID {dto.CourseId.Value} not found");
                    }
                }

                // Store old logo path for potential deletion
                var oldLogoPath = batch.BatchLogoUrl;

                // Update batch properties
                batch.BatchName = dto.BatchName;
                batch.BatchStartDate = dto.BatchStartDate;
                batch.BatchEndDate = dto.BatchEndDate;

                if (dto.CourseId.HasValue)
                {
                    batch.CourseId = dto.CourseId.Value;
                }

                batch.PresentationDate = dto.PresentationDate;
                batch.FinalPresentationDate = dto.FinalPresentationDate;
                batch.CourseFees = dto.CourseFees;
                batch.CourseFeesFrom = dto.CourseFeesFrom;
                batch.CourseFeesTo = dto.CourseFeesTo;

                if (dto.BatchIsActive.HasValue)
                {
                    batch.BatchIsActive = dto.BatchIsActive.Value;
                }

                // Handle logo update
                if (dto.BatchLogo != null && dto.BatchLogo.Length > 0)
                {
                    // Delete old logo if exists
                    if (!string.IsNullOrEmpty(oldLogoPath))
                    {
                        await _fileStorageService.DeleteFileAsync(oldLogoPath);
                    }

                    // Upload new logo
                    var logoUrl = await _fileStorageService.StoreImageAsync(dto.BatchLogo,UploadType.Batch,dto.BatchName);

                    batch.BatchLogoUrl = logoUrl;
                }

                // Update in database
                var updatedBatch = await _batchRepository.UpdateAsync(batch);

                // Map to response DTO
                return MapToDTO(updatedBatch);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating batch: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<BatchResponseDTO>> GetAllBatchesAsync()
        {
            var batches = await _batchRepository.GetAllAsync();
            return batches.Select(MapToDTO);
        }

        public async Task<IEnumerable<BatchResponseDTO>> GetActiveBatchesAsync()
        {
            var batches = await _batchRepository.GetActiveBatchesAsync();
            return batches.Select(MapToDTO);
        }

        public async Task<IEnumerable<BatchResponseDTO>> GetBatchesByCourseIdAsync(int courseId)
        {
            var batches = await _batchRepository.GetBatchesByCourseIdAsync(courseId);
            return batches.Select(MapToDTO);
        }

        public async Task<IEnumerable<BatchResponseDTO>> SearchBatchesAsync(string keyword)
        {
            var batches = await _batchRepository.SearchBatchesAsync(keyword);
            return batches.Select(MapToDTO);
        }

        public async Task<BatchResponseDTO> GetBatchByIdAsync(int batchId)
        {
            var batch = await _batchRepository.GetByIdAsync(batchId);
            if (batch == null)
            {
                throw new KeyNotFoundException($"Batch with ID {batchId} not found");
            }
            return MapToDTO(batch);
        }

        public async Task<Batch> GetBatchEntityByIdAsync(int batchId)
        {
            var batch = await _batchRepository.GetByIdAsync(batchId);
            if (batch == null)
            {
                throw new KeyNotFoundException($"Batch with ID {batchId} not found");
            }
            return batch;
        }

        public async Task<bool> DeleteBatchAsync(int batchId)
        {
            var batch = await _batchRepository.GetByIdAsync(batchId);
            if (batch == null)
            {
                throw new KeyNotFoundException($"Batch with ID {batchId} not found");
            }

            // Delete associated logo if exists
            if (!string.IsNullOrEmpty(batch.BatchLogoUrl))
            {
                await _fileStorageService.DeleteFileAsync(batch.BatchLogoUrl);
            }

            return await _batchRepository.DeleteAsync(batchId);
        }

        // Helper method to store batch images
        private async Task<string> StoreBatchImageAsync(Microsoft.AspNetCore.Http.IFormFile file, string batchName)
        {
            if (file == null || file.Length == 0)
                return null;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
                throw new InvalidOperationException("Invalid file type. Only image files are allowed.");

            if (file.Length > 5 * 1024 * 1024)
                throw new InvalidOperationException("File size exceeds 5MB limit.");

            var sanitizedBatchName = SanitizeFileName(batchName);
            var uniqueFileName = $"{sanitizedBatchName}_{Guid.NewGuid()}{fileExtension}";

            // Use a different subdirectory for batch logos
            var uploadPath = System.IO.Path.Combine("wwwroot", "uploads", "batches");
            if (!System.IO.Directory.Exists(uploadPath))
                System.IO.Directory.CreateDirectory(uploadPath);

            var filePath = System.IO.Path.Combine(uploadPath, uniqueFileName);

            using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/batches/{uniqueFileName}";
        }

        private string SanitizeFileName(string fileName)
        {
            var invalidChars = System.IO.Path.GetInvalidFileNameChars();
            var sanitized = string.Join("_", fileName.Split(invalidChars));
            return sanitized.Length > 50 ? sanitized.Substring(0, 50) : sanitized;
        }

        private BatchResponseDTO MapToDTO(Batch batch)
        {
            return new BatchResponseDTO
            {
                BatchId = batch.BatchId,
                BatchName = batch.BatchName,
                BatchStartDate = batch.BatchStartDate,
                BatchEndDate = batch.BatchEndDate,
                CourseId = batch.CourseId,
                CourseName = batch.Course?.CourseName,
                PresentationDate = batch.PresentationDate,
                CourseFees = batch.CourseFees,
                CourseFeesFrom = batch.CourseFeesFrom,
                CourseFeesTo = batch.CourseFeesTo,
                BatchIsActive = batch.BatchIsActive,
                FinalPresentationDate = batch.FinalPresentationDate,
                BatchLogoUrl = batch.BatchLogoUrl,
                CreatedAt = batch.CreatedAt,
                UpdatedAt = batch.UpdatedAt
            };
        }
    }
}
