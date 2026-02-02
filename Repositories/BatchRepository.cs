using ComputerSeekho.API.Data;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ComputerSeekho.API.Repositories
{
    public class BatchRepository : IBatchRepository
    {
        private readonly AppDbContext _context;

        public BatchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Batch> GetByIdAsync(int id)
        {
            return await _context.Batches
                .Include(b => b.Course)  // Eager load the Course
                .FirstOrDefaultAsync(b => b.BatchId == id);
        }

        public async Task<IEnumerable<Batch>> GetAllAsync()
        {
            return await _context.Batches
                .Include(b => b.Course)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Batch>> GetActiveBatchesAsync()
        {
            return await _context.Batches
                .Include(b => b.Course)
                .Where(b => b.BatchIsActive == true)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Batch>> GetBatchesByCourseIdAsync(int courseId)
        {
            return await _context.Batches
                .Include(b => b.Course)
                .Where(b => b.CourseId == courseId && b.BatchIsActive == true)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Batch>> SearchBatchesAsync(string keyword)
        {
            return await _context.Batches
                .Include(b => b.Course)
                .Where(b => b.BatchName.Contains(keyword))
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<Batch> AddAsync(Batch batch)
        {
            await _context.Batches.AddAsync(batch);
            await _context.SaveChangesAsync();

            // Reload with Course relationship
            return await GetByIdAsync(batch.BatchId);
        }

        public async Task<Batch> UpdateAsync(Batch batch)
        {
            _context.Batches.Update(batch);
            await _context.SaveChangesAsync();

            // Reload with Course relationship
            return await GetByIdAsync(batch.BatchId);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var batch = await GetByIdAsync(id);
            if (batch == null)
                return false;

            _context.Batches.Remove(batch);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Batches.AnyAsync(b => b.BatchId == id);
        }
    }
}
