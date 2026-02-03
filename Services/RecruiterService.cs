using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Enum;
using ComputerSeekho.API.Repositories.Interfaces;
using ComputerSeekho.Application.Services.Interfaces;

namespace ComputerSeekho.Application.Services
{
    public class RecruiterService : IRecruiterService
    {
        private readonly IRecruiterRepository _repository;
        private readonly IFileStorageService _fileStorage;

        public RecruiterService(
            IRecruiterRepository repository,
            IFileStorageService fileStorage)
        {
            _repository = repository;
            _fileStorage = fileStorage;
        }

        // GET
        public async Task<List<RecruiterMaster>> GetAllAsync()
            => await _repository.GetAllAsync();

        // POST
        public async Task CreateAsync(RecruiterDto dto)
        {
            string logoUrl = null;

            if (dto.Logo != null)
            {
                logoUrl = await _fileStorage.StoreImageAsync(
                    dto.Logo,
                    UploadType.Recruiter,
                    dto.RecruiterName
                );
            }

            var recruiter = new RecruiterMaster
            {
                RecruiterName = dto.RecruiterName,
                LogoUrl = logoUrl,
                Status = dto.Status
            };

            await _repository.AddAsync(recruiter);
        }

        // PUT
        public async Task UpdateAsync(int id, RecruiterDto dto)
        {
            var recruiter = await _repository.GetByIdAsync(id);
            if (recruiter == null)
                throw new Exception("Recruiter not found");

            if (dto.Logo != null && !string.IsNullOrEmpty(recruiter.LogoUrl))
            {
                await _fileStorage.DeleteFileAsync(recruiter.LogoUrl);

                recruiter.LogoUrl = await _fileStorage.StoreImageAsync(
                    dto.Logo,
                    UploadType.Recruiter,
                    dto.RecruiterName
                );
            }

            recruiter.RecruiterName = dto.RecruiterName;
            recruiter.Status = dto.Status;

            await _repository.UpdateAsync(recruiter);
        }

        // DELETE
        public async Task DeleteAsync(int id)
        {
            var recruiter = await _repository.GetByIdAsync(id);
            if (recruiter == null)
                throw new Exception("Recruiter not found");

            if (!string.IsNullOrEmpty(recruiter.LogoUrl))
                await _fileStorage.DeleteFileAsync(recruiter.LogoUrl);

            await _repository.DeleteAsync(recruiter);
        }

        public async Task<List<RecruiterLogoDto>> GetActiveLogosAsync()
        {
            var recruiters = await _repository.GetActiveAsync();

            return recruiters.Select(r => new RecruiterLogoDto
            {
                RecruiterId = r.RecruiterId,
                LogoUrl = r.LogoUrl
            }).ToList();
        }
    }
}
