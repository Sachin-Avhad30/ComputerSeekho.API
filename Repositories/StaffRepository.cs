using ComputerSeekho.API.Data;
using ComputerSeekho.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComputerSeekho.API.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly AppDbContext _context;

        public StaffRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Staff?> GetByUsernameAsync(string username)
        {
            return await _context.Staff
                .FirstOrDefaultAsync(s => s.StaffUsername == username);
        }

        public async Task<Staff?> GetByEmailAsync(string email)
        {
            return await _context.Staff
                .FirstOrDefaultAsync(s => s.StaffEmail == email);
        }

        public async Task<Staff?> GetByIdAsync(int id)
        {
            return await _context.Staff.FindAsync(id);
        }

        public async Task<List<Staff>> GetAllAsync()
        {
            return await _context.Staff.ToListAsync();
        }

        public async Task<List<Staff>> GetByRoleAsync(string role)
        {
            return await _context.Staff
                .Where(s => s.StaffRole == role)
                .ToListAsync();
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Staff
                .AnyAsync(s => s.StaffUsername == username);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Staff
                .AnyAsync(s => s.StaffEmail == email);
        }

        public async Task<Staff> AddAsync(Staff staff)
        {
            _context.Staff.Add(staff);
            await _context.SaveChangesAsync();
            return staff;
        }

        public async Task<Staff> UpdateAsync(Staff staff)
        {
            _context.Staff.Update(staff);
            await _context.SaveChangesAsync();
            return staff;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var staff = await GetByIdAsync(id);
            if (staff == null) return false;

            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}