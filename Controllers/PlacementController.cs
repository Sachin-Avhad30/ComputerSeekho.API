using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Services.Interfaces;
using ComputerSeekho.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSeekho.API.Controllers
{
    [ApiController]
    [Route("api/placements")]
    public class PlacementController : ControllerBase
    {
        private readonly IPlacementService _service;

        public PlacementController(IPlacementService service)
        {
            _service = service;
        }

        // 1️⃣ GET ALL PLACEMENTS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        // 2️⃣ POST - CREATE PLACEMENT
        [HttpPost]
        public async Task<IActionResult> Create(PlacementDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Placement created successfully");
        }

        // 3️⃣ PUT - UPDATE PLACEMENT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PlacementDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok("Placement updated successfully");
        }

        // 4️⃣ DELETE - DELETE PLACEMENT
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Placement deleted successfully");
        }

        // ✅ GET STUDENTS PLACED IN A RECRUITER
        [HttpGet("{recruiterId}/placed-students")]
        public async Task<IActionResult> GetPlacedStudents(int recruiterId)
        {
            var result = await _service.GetPlacedStudentPhotosAsync(recruiterId);
            return Ok(result);
        }
    }
}
