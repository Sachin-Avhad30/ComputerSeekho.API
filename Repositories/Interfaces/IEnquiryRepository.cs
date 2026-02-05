using ComputerSeekho.API.Entities;

namespace ComputerSeekho.API.Repositories.Interfaces
{
    public interface IEnquiryRepository
    {
        Task<Enquiry> CreateAsync(Enquiry enquiry);
        Task<Enquiry?> GetByIdAsync(int id);
        Task<Enquiry> UpdateAsync(Enquiry enquiry);
        Task<List<Enquiry>> GetUpcomingFollowupsForStaffAsync(int staffId, DateTime fromDate);
        Task<List<Enquiry>> GetAllPendingFollowupsAsync(DateTime fromDate);
    }
}