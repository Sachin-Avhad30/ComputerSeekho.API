using ComputerSeekho.API.Data;
using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Repositories;
using ComputerSeekho.API.Repositories.Interfaces;
using ComputerSeekho.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComputerSeekho.API.Services
{
    public class PlacementService : IPlacementService
    {
        private readonly IPlacementRepository _repository;
        private readonly AppDbContext _context;


        public PlacementService(IPlacementRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        // GET
        public async Task<List<PlacementMaster>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // POST
        public async Task CreateAsync(PlacementDto dto)
        {
            var placement = new PlacementMaster
            {
                StudentId = dto.StudentId,
                BatchId = dto.BatchId,
                RecruiterId = dto.RecruiterId
            };

            await _repository.AddAsync(placement);
        }

        // PUT
        public async Task UpdateAsync(int id, PlacementDto dto)
        {
            var placement = await _repository.GetByIdAsync(id);
            if (placement == null)
                throw new Exception("Placement not found");

            placement.StudentId = dto.StudentId;
            placement.BatchId = dto.BatchId;
            placement.RecruiterId = dto.RecruiterId;

            await _repository.UpdateAsync(placement);
        }

        // DELETE
        public async Task DeleteAsync(int id)
        {
            var placement = await _repository.GetByIdAsync(id);
            if (placement == null)
                throw new Exception("Placement not found");

            await _repository.DeleteAsync(placement);
        }

        public async Task<List<PlacedStudentPhotoDto>> GetPlacedStudentPhotosAsync(int recruiterId)
        {
            var placements = await _repository
                .GetPlacementsByRecruiterAsync(recruiterId);

            var studentIds = placements
                .Select(p => p.StudentId)
                .Distinct()
                .ToList();

            return await _context.StudentMasters
                .AsNoTracking()
                .Where(s => studentIds.Contains(s.StudentId)
                            && s.PhotoUrl != null)
                .Select(s => new PlacedStudentPhotoDto
                {
                    StudentId = s.StudentId,
                    PhotoUrl = s.PhotoUrl
                })
                .ToListAsync();
        }
    }
}
