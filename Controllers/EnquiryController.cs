using ComputerSeekho.API.DTOs.Request;
using ComputerSeekho.API.DTOs.Response;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSeekho.API.Controllers
{
    [ApiController]
    [Route("api/enquiries")]
    [EnableCors("AllowFrontend")]
    public class EnquiryController : ControllerBase
    {
        private readonly IEnquiryService _enquiryService;

        public EnquiryController(IEnquiryService enquiryService)
        {
            _enquiryService = enquiryService;
        }

        /// <summary>
        /// Add Enquiry Page - Create new enquiry
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Enquiry>> CreateEnquiry([FromBody] EnquiryCreateRequestDTO dto)
        {
            try
            {
                var enquiry = await _enquiryService.CreateEnquiryAsync(dto);
                return Ok(enquiry);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get Single Enquiry by ID (for Edit functionality)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Enquiry>> GetEnquiryById(int id)
        {
            try
            {
                var enquiry = await _enquiryService.GetEnquiryByIdAsync(id);
                return Ok(enquiry);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update Enquiry (Edit functionality)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<Enquiry>> UpdateEnquiry(int id, [FromBody] EnquiryUpdateRequestDTO dto)
        {
            try
            {
                var updatedEnquiry = await _enquiryService.UpdateEnquiryAsync(id, dto);
                return Ok(updatedEnquiry);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Follow-ups for logged-in staff (TODAY + pending)
        /// </summary>
        [HttpGet("followups/staff/{staffId}")]
        public async Task<ActionResult<List<EnquiryResponseDTO>>> GetUpcomingFollowups(int staffId)
        {
            try
            {
                var followups = await _enquiryService.GetUpcomingFollowupsForStaffAsync(staffId);
                return Ok(followups);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// View All follow-ups (Admin)
        /// </summary>
        [HttpGet("followups/all")]
        public async Task<ActionResult<List<EnquiryResponseDTO>>> GetAllFollowups()
        {
            try
            {
                var followups = await _enquiryService.GetAllPendingFollowupsAsync();
                return Ok(followups);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// CALL button action (Follow-up update)
        /// </summary>
        [HttpPut("followup")]
        public async Task<ActionResult<Enquiry>> UpdateFollowup([FromBody] EnquiryFollowUpRequestDTO dto)
        {
            try
            {
                var enquiry = await _enquiryService.UpdateFollowupAsync(dto);
                return Ok(enquiry);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}