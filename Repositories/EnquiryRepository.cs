using ComputerSeekho.API.Data;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComputerSeekho.API.Repositories
{
    public class EnquiryRepository : IEnquiryRepository
    {
        private readonly AppDbContext _context;

        public EnquiryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Enquiry> CreateAsync(Enquiry enquiry)
        {
            await _context.Set<Enquiry>().AddAsync(enquiry);
            await _context.SaveChangesAsync();
            return enquiry;
        }

        public async Task<Enquiry?> GetByIdAsync(int id)
        {
            return await _context.Set<Enquiry>()
                .Include(e => e.Course)
                .Include(e => e.Staff)
                .Include(e => e.ClosureReason)
                .FirstOrDefaultAsync(e => e.EnquiryId == id);
        }

        public async Task<Enquiry> UpdateAsync(Enquiry enquiry)
        {
            _context.Set<Enquiry>().Update(enquiry);
            await _context.SaveChangesAsync();
            return enquiry;
        }

        public async Task<List<Enquiry>> GetUpcomingFollowupsForStaffAsync(int staffId, DateTime fromDate)
        {
            return await _context.Set<Enquiry>()
                .Include(e => e.Course)
                .Include(e => e.Staff)
                .Where(e => e.StaffId == staffId
                    && !e.IsClosed
                    && !e.StudentId.HasValue  // ✅ Exclude converted enquiries
                    && e.FollowupDate.HasValue
                    && e.FollowupDate.Value.Date >= fromDate.Date)
                .OrderBy(e => e.FollowupDate)
                .ToListAsync();
        }

        public async Task<List<Enquiry>> GetAllPendingFollowupsAsync(DateTime fromDate)
        {
            return await _context.Set<Enquiry>()
                .Include(e => e.Course)
                .Include(e => e.Staff)
                .Where(e => !e.IsClosed
                    && !e.StudentId.HasValue  // ✅ Exclude converted enquiries
                    && e.FollowupDate.HasValue
                    && e.FollowupDate.Value.Date >= fromDate.Date)
                .OrderBy(e => e.FollowupDate)
                .ToListAsync();
        }
    }
}