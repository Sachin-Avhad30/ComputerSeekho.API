using ComputerSeekho.API.Data;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComputerSeekho.API.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly AppDbContext _context;

        public AnnouncementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Announcement> GetByIdAsync(int id)
        {
            return await _context.Announcements
                .FirstOrDefaultAsync(a => a.AnnouncementId == id);
        }

        public async Task<IEnumerable<Announcement>> GetAllAsync()
        {
            return await _context.Announcements
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetActiveAnnouncementTextsAsync(DateTime currentDateTime)
        {
            return await _context.Announcements
                .Where(a => a.IsActive == true &&
                           (a.ValidFrom == null || a.ValidFrom <= currentDateTime) &&
                           (a.ValidTo == null || a.ValidTo >= currentDateTime))
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => a.AnnouncementText)
                .ToListAsync();
        }

        public async Task<Announcement> AddAsync(Announcement announcement)
        {
            await _context.Announcements.AddAsync(announcement);
            await _context.SaveChangesAsync();
            return announcement;
        }

        public async Task<Announcement> UpdateAsync(Announcement announcement)
        {
            _context.Announcements.Update(announcement);
            await _context.SaveChangesAsync();
            return announcement;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var announcement = await GetByIdAsync(id);
            if (announcement == null)
                return false;

            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Announcements.AnyAsync(a => a.AnnouncementId == id);
        }
    }
}
