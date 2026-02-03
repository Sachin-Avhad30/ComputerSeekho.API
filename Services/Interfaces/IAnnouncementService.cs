using ComputerSeekho.API.DTOs;

namespace ComputerSeekho.API.Services.Interfaces
{
    public interface IAnnouncementService
    {
        Task<IEnumerable<string>> GetActiveAnnouncementTextsAsync();
        Task<IEnumerable<AnnouncementResponseDTO>> GetAllAnnouncementsAsync();
        Task<AnnouncementResponseDTO> GetAnnouncementByIdAsync(int id);
        Task<AnnouncementResponseDTO> CreateAnnouncementAsync(AnnouncementCreateRequestDTO dto);
        Task<AnnouncementResponseDTO> UpdateAnnouncementAsync(int id, AnnouncementUpdateRequestDTO dto);
        Task<bool> DeleteAnnouncementAsync(int id);
    }
}
