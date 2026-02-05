using ComputerSeekho.API.DTOs.Request;
using ComputerSeekho.API.DTOs.Response;
using ComputerSeekho.API.Entities;

namespace ComputerSeekho.API.Services.Interfaces
{
    public interface IEnquiryService
    {
        Task<Enquiry> CreateEnquiryAsync(EnquiryCreateRequestDTO dto);
        Task<Enquiry> GetEnquiryByIdAsync(int id);
        Task<Enquiry> UpdateEnquiryAsync(int id, EnquiryUpdateRequestDTO dto);
        Task<List<EnquiryResponseDTO>> GetUpcomingFollowupsForStaffAsync(int staffId);
        Task<List<EnquiryResponseDTO>> GetAllPendingFollowupsAsync();
        Task<Enquiry> UpdateFollowupAsync(EnquiryFollowUpRequestDTO dto);
    }
}