using ComputerSeekho.API.Data;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ComputerSeekho.API.Repositories
{
    public class RecruiterRepository : IRecruiterRepository
    {
        private readonly AppDbContext _context;

        public RecruiterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RecruiterMaster>> GetAllAsync()
         => await _context.RecruiterMasters.ToListAsync();

        public async Task<RecruiterMaster> GetByIdAsync(int id)
            => await _context.RecruiterMasters.FindAsync(id);

        public async Task AddAsync(RecruiterMaster recruiter)
        {
            _context.RecruiterMasters.Add(recruiter);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RecruiterMaster recruiter)
        {
            _context.RecruiterMasters.Update(recruiter);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(RecruiterMaster recruiter)
        {
            _context.RecruiterMasters.Remove(recruiter);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RecruiterMaster>> GetActiveAsync()
        {
            return await _context.RecruiterMasters
                .AsNoTracking()
                .Where(r => r.Status == true && r.LogoUrl != null)
                .ToListAsync();
        }

    }
}
