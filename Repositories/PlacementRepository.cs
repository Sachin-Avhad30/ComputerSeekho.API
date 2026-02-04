using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Repositories.Interfaces;
using ComputerSeekho.API.Data;
using Microsoft.EntityFrameworkCore;

namespace ComputerSeekho.API.Repositories
{
    public class PlacementRepository : IPlacementRepository
    {
        private readonly AppDbContext _context;

        public PlacementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Placement>> GetAllAsync()
        {
            return await _context.PlacementMasters
                                 .Include(p => p.Recruiter)
                                 .ToListAsync();
        }

        public async Task<Placement?> GetByIdAsync(int id)
        {
            return await _context.PlacementMasters
                                 .Include(p => p.Recruiter)
                                 .FirstOrDefaultAsync(p => p.PlacementId == id);
        }

        public async Task AddAsync(Placement placement)
        {
            _context.PlacementMasters.Add(placement);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Placement placement)
        {
            _context.PlacementMasters.Update(placement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Placement placement)
        {
            _context.PlacementMasters.Remove(placement);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Placement>> GetPlacementsByRecruiterAsync(int recruiterId)
        {
            return await _context.PlacementMasters
                .AsNoTracking()
                .Where(p => p.RecruiterId == recruiterId)
                .ToListAsync();
        }
        // NEW METHOD
        public async Task<List<Placement>> GetPlacementsByBatchAsync(int batchId)
        {
            return await _context.PlacementMasters
                .AsNoTracking()
                .Include(p => p.Student)
                .Include(p => p.Recruiter)
                .Include(p => p.Batch)
                .Where(p => p.BatchId == batchId)
                .ToListAsync();
        }
    }
}
