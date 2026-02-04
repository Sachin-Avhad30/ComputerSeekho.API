using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComputerSeekho.API.Security.Attribuite;
using Intercom.Core;

namespace ComputerSeekho.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class StaffAuthController : ControllerBase
    {
        private readonly IStaffAuthService _staffAuthService;

        public StaffAuthController(IStaffAuthService staffAuthService)
        {
            _staffAuthService = staffAuthService;
        }

        /// <summary>
        /// Staff Login endpoint
        /// POST /api/auth/login
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateStaff([FromBody] StaffLoginRequest loginRequest)
        {
            try
            {
                var jwtResponse = await _staffAuthService.AuthenticateStaffAsync(loginRequest);
                return Ok(jwtResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Staff Signup/Register endpoint with IMAGE UPLOAD
        /// POST /api/auth/signup
        /// Accepts multipart/form-data
        /// </summary>
        [HttpPost("signup")]
        public async Task<IActionResult> RegisterStaff([FromForm] StaffSignupRequest signupRequest)
        {
            try
            {
                var messageResponse = await _staffAuthService.RegisterStaffWithImageAsync(signupRequest);

                // Check if registration was successful
                if (messageResponse.Message.StartsWith("Error"))
                {
                    return BadRequest(messageResponse);
                }

                return Ok(messageResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get list of all faculty members (teaching staff)
        /// GET /api/auth/faculty
        /// </summary>
        [HttpGet("faculty")]
        public async Task<IActionResult> GetFacultyList()
        {
            try
            {
                var facultyList = await _staffAuthService.GetFacultyListAsync();
                return Ok(facultyList);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get all staff members (admin only)
        /// GET /api/auth/staff
        /// </summary>
        [HttpGet("staff")]
        [Security.Attribuite.Authorize("admin")]
        public async Task<IActionResult> GetAllStaff()
        {
            try
            {
                var staffList = await _staffAuthService.GetAllStaffAsync();
                return Ok(staffList);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get staff by ID (authenticated users only)
        /// GET /api/auth/staff/{id}
        /// </summary>
        [HttpGet("staff/{id}")]
        [Security.Attribuite.Authorize]
        public async Task<IActionResult> GetStaffById(int id)
        {
            try
            {
                var staff = await _staffAuthService.GetStaffByIdAsync(id);

                if (staff == null)
                {
                    return NotFound(new MessageResponse("Staff not found"));
                }

                return Ok(staff);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponse($"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get current logged-in staff info
        /// GET /api/auth/me
        /// </summary>
        //[HttpGet("me")]
        //[Security.Attribuite.Authorize]
        //public async Task<IActionResult> GetCurrentStaff()
        //{
        //    try
        //    {
        //        var staff = HttpContext.Items["Staff"];

        //        if (staff == null)
        //        {
        //            return Unauthorized(new MessageResponse("Unauthorized"));
        //        }

        //        var staffInfo = await _staffAuthService.GetStaffByIdAsync(staffId);
        //        return Ok(staffInfo);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new MessageResponse($"Error: {ex.Message}"));
        //    }
        //}
    }
}
