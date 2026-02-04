using ComputerSeekho.API.Entities;
using ComputerSeekho.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSeekho.API.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        private readonly ILogger<StaffController> _logger;

        public StaffController(IStaffService staffService, ILogger<StaffController> logger)
        {
            _staffService = staffService;
            _logger = logger;
        }

        /// <summary>
        /// Get all teaching staff (faculty)
        /// GET /api/staff/faculty
        /// </summary>
        [HttpGet("faculty")]
        public async Task<IActionResult> GetAllFaculty()
        {
            try
            {
                var faculty = await _staffService.GetFacultyListAsync();
                return Ok(faculty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving faculty list");
                return StatusCode(500, "An error occurred while retrieving faculty list");
            }
        }

        /// <summary>
        /// Get all staff
        /// GET /api/staff/all
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllStaff()
        {
            try
            {
                var staff = await _staffService.GetAllStaffAsync();
                return Ok(staff);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving staff list");
                return StatusCode(500, "An error occurred while retrieving staff list");
            }
        }

        /// <summary>
        /// Get staff by ID
        /// GET /api/staff/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaffById(int id)
        {
            try
            {
                var staff = await _staffService.GetStaffByIdAsync(id);
                return Ok(staff);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving staff with id: {Id}", id);
                return NotFound($"Staff not found with id: {id}");
            }
        }
    }
}