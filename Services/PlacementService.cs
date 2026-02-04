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
        public async Task<List<Placement>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // POST
        public async Task CreateAsync(PlacementDto dto)
        {
            var placement = new Placement
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
            return await (
        from p in _context.PlacementMasters
        join s in _context.StudentMasters on p.StudentId equals s.StudentId
        join r in _context.RecruiterMasters on p.RecruiterId equals r.RecruiterId
        where p.RecruiterId == recruiterId
              && s.PhotoUrl != null
        select new PlacedStudentPhotoDto
        {
            StudentId = s.StudentId,
            StudentName = s.StudentName,
            PhotoUrl = s.PhotoUrl,
            comapnyName = r.RecruiterName
        }
    )
    .AsNoTracking()
    .Distinct()
    .ToListAsync();
        }
    

    // NEW METHOD - Get all batches with placement count
        public async Task<List<BatchPlacementSummaryDto>> GetBatchesWithPlacementsAsync()
        {
            return await (
                from b in _context.Batches
                join p in _context.PlacementMasters on b.BatchId equals p.BatchId
                group p by new { b.BatchId, b.BatchName, b.BatchLogoUrl } into g
                select new BatchPlacementSummaryDto
                {
                    BatchId = g.Key.BatchId,
                    BatchName = g.Key.BatchName,
                    BatchLogo = g.Key.BatchLogoUrl,
                    PlacedStudentCount = g.Count()
                }
            )
            .AsNoTracking()
            .OrderByDescending(x => x.PlacedStudentCount)
            .ToListAsync();
        }

        // NEW METHOD - Get all placed students for a specific batch
        public async Task<List<PlacedStudentDetailDto>> GetPlacedStudentsByBatchAsync(int batchId)
        {
            return await (
                from p in _context.PlacementMasters
                join s in _context.StudentMasters on p.StudentId equals s.StudentId
                join r in _context.RecruiterMasters on p.RecruiterId equals r.RecruiterId
                join b in _context.Batches on p.BatchId equals b.BatchId
                where p.BatchId == batchId
                select new PlacedStudentDetailDto
                {
                    StudentId = s.StudentId,
                    StudentName = s.StudentName,
                    PhotoUrl = s.PhotoUrl,
                    CompanyName = r.RecruiterName,
                    CompanyLogo = r.LogoUrl,
                    BatchId = b.BatchId,
                    BatchName = b.BatchName
                }
            )
            .AsNoTracking()
            .ToListAsync();
        }
    }
}
