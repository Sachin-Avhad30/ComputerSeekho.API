
using ComputerSeekho.Application.DTOs;
using ComputerSeekho.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSeekho.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICourseService courseService, ILogger<CoursesController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CourseResponseDTO>> CreateCourse([FromForm] CourseCreateRequestDTO dto)
        {
            try
            {
                var result = await _courseService.CreateCourseWithImageAsync(dto);
                return CreatedAtAction(nameof(GetCourseById), new { courseId = result.CourseId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{courseId}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CourseResponseDTO>> UpdateCourse(
            int courseId, [FromForm] CourseUpdateRequestDTO dto)
        {
            try
            {
                var result = await _courseService.UpdateCourseWithImageAsync(courseId, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating course");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseResponseDTO>>> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<CourseResponseDTO>>> GetActiveCourses()
        {
            var courses = await _courseService.GetActiveCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("{courseId}")]
        public async Task<ActionResult<CourseResponseDTO>> GetCourseById(int courseId)
        {
            try
            {
                var course = await _courseService.GetCourseByIdAsync(courseId);
                return Ok(course);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            try
            {
                await _courseService.DeleteCourseAsync(courseId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet("name/{id}")]
        public async Task<IActionResult> GetCourseNameById(int id)
        {
            var courseName = await _courseService.GetCourseNameByIdAsync(id);

            if (courseName == null)
                return NotFound("Course not found");

            return Ok(new { courseName });
        }
    }
}
