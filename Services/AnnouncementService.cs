using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Repositories.Interfaces;
using ComputerSeekho.API.Services.Interfaces;

namespace ComputerSeekho.API.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public AnnouncementService(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public async Task<IEnumerable<string>> GetActiveAnnouncementTextsAsync()
        {
            var currentDateTime = DateTime.Now;
            return await _announcementRepository.GetActiveAnnouncementTextsAsync(currentDateTime);
        }

        public async Task<IEnumerable<AnnouncementResponseDTO>> GetAllAnnouncementsAsync()
        {
            var announcements = await _announcementRepository.GetAllAsync();
            return announcements.Select(MapToDTO);
        }

        public async Task<AnnouncementResponseDTO> GetAnnouncementByIdAsync(int id)
        {
            var announcement = await _announcementRepository.GetByIdAsync(id);
            if (announcement == null)
            {
                throw new KeyNotFoundException($"Announcement with ID {id} not found");
            }
            return MapToDTO(announcement);
        }

        public async Task<AnnouncementResponseDTO> CreateAnnouncementAsync(AnnouncementCreateRequestDTO dto)
        {
            try
            {
                var announcement = new Announcement
                {
                    AnnouncementText = dto.AnnouncementText,
                    ValidFrom = dto.ValidFrom,
                    ValidTo = dto.ValidTo,
                    IsActive = dto.IsActive ?? true
                };

                var savedAnnouncement = await _announcementRepository.AddAsync(announcement);
                return MapToDTO(savedAnnouncement);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating announcement: {ex.Message}", ex);
            }
        }

        public async Task<AnnouncementResponseDTO> UpdateAnnouncementAsync(
            int id,
            AnnouncementUpdateRequestDTO dto)
        {
            try
            {
                var announcement = await _announcementRepository.GetByIdAsync(id);
                if (announcement == null)
                {
                    throw new KeyNotFoundException($"Announcement with ID {id} not found");
                }

                // Update properties
                announcement.AnnouncementText = dto.AnnouncementText;
                announcement.ValidFrom = dto.ValidFrom;
                announcement.ValidTo = dto.ValidTo;

                if (dto.IsActive.HasValue)
                {
                    announcement.IsActive = dto.IsActive.Value;
                }

                var updatedAnnouncement = await _announcementRepository.UpdateAsync(announcement);
                return MapToDTO(updatedAnnouncement);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating announcement: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAnnouncementAsync(int id)
        {
            var exists = await _announcementRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Announcement with ID {id} not found");
            }

            return await _announcementRepository.DeleteAsync(id);
        }

        private AnnouncementResponseDTO MapToDTO(Announcement announcement)
        {
            return new AnnouncementResponseDTO
            {
                AnnouncementId = announcement.AnnouncementId,
                AnnouncementText = announcement.AnnouncementText,
                ValidFrom = announcement.ValidFrom,
                ValidTo = announcement.ValidTo,
                IsActive = announcement.IsActive,
                CreatedAt = announcement.CreatedAt,
                UpdatedAt = announcement.UpdatedAt
            };
        }
    }
}
