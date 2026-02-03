using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSeekho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;
        private readonly ILogger<AnnouncementsController> _logger;

        public AnnouncementsController(
            IAnnouncementService announcementService,
            ILogger<AnnouncementsController> logger)
        {
            _announcementService = announcementService;
            _logger = logger;
        }

        /// <summary>
        /// Get active announcement texts only (for display on homepage/banner)
        /// Returns only the text of active announcements within their validity period
        /// </summary>
        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<string>>> GetActiveAnnouncements()
        {
            try
            {
                var announcements = await _announcementService.GetActiveAnnouncementTextsAsync();
                return Ok(announcements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active announcements");
                return StatusCode(500, new { message = "An error occurred while retrieving active announcements" });
            }
        }

        /// <summary>
        /// Get all announcements (for admin panel)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AnnouncementResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AnnouncementResponseDTO>>> GetAllAnnouncements()
        {
            try
            {
                var announcements = await _announcementService.GetAllAnnouncementsAsync();
                return Ok(announcements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all announcements");
                return StatusCode(500, new { message = "An error occurred while retrieving announcements" });
            }
        }

        /// <summary>
        /// Get announcement by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AnnouncementResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AnnouncementResponseDTO>> GetAnnouncementById(int id)
        {
            try
            {
                var announcement = await _announcementService.GetAnnouncementByIdAsync(id);
                return Ok(announcement);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Announcement not found: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving announcement {Id}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving the announcement" });
            }
        }

        /// <summary>
        /// Create a new announcement
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AnnouncementResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AnnouncementResponseDTO>> CreateAnnouncement(
            [FromBody] AnnouncementCreateRequestDTO dto)
        {
            try
            {
                var result = await _announcementService.CreateAnnouncementAsync(dto);
                return CreatedAtAction(
                    nameof(GetAnnouncementById),
                    new { id = result.AnnouncementId },
                    result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating announcement");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing announcement
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AnnouncementResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AnnouncementResponseDTO>> UpdateAnnouncement(
            int id,
            [FromBody] AnnouncementUpdateRequestDTO dto)
        {
            try
            {
                var result = await _announcementService.UpdateAnnouncementAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Announcement not found: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating announcement {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete an announcement
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            try
            {
                await _announcementService.DeleteAnnouncementAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Announcement not found: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting announcement {Id}", id);
                return StatusCode(500, new { message = "An error occurred while deleting the announcement" });
            }
        }
    }
}
