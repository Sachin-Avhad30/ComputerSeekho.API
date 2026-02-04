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

        public async Task<List<Recruiter>> GetAllAsync()
         => await _context.RecruiterMasters.ToListAsync();

        public async Task<Recruiter> GetByIdAsync(int id)
            => await _context.RecruiterMasters.FindAsync(id);

        public async Task AddAsync(Recruiter recruiter)
        {
            _context.RecruiterMasters.Add(recruiter);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Recruiter recruiter)
        {
            _context.RecruiterMasters.Update(recruiter);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Recruiter recruiter)
        {
            _context.RecruiterMasters.Remove(recruiter);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Recruiter>> GetActiveAsync()
        {
            return await _context.RecruiterMasters
                .AsNoTracking()
                .Where(r => r.Status == true && r.LogoUrl != null)
                .ToListAsync();
        }

    }
}
