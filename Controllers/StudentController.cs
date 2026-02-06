using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Services.Interfaces;
using ComputerSeekho.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSeekho.API.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }

        // 1️⃣ GET ALL STUDENTS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        // 🔥 POST (FORM DATA)
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StudentDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Student created successfully");
        }

        // 🔥 PUT (FORM DATA)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] StudentDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok("Student updated successfully");
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Student deleted successfully");
        }

       
    }
}
