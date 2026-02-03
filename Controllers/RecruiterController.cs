using ComputerSeekho.API.DTOs;
using ComputerSeekho.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSeekho.API.Controllers
{
    [ApiController]
    [Route("api/recruiters")]
    public class RecruiterController : ControllerBase
    {
        private readonly IRecruiterService _service;

        public RecruiterController(IRecruiterService service)
        {
            _service = service;
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());
        
        
        // ✅ GET ACTIVE RECRUITER LOGOS ONLY
        [HttpGet("logos/active")]
        public async Task<IActionResult> GetActiveRecruiterLogos()
        {
            return Ok(await _service.GetActiveLogosAsync());
        }


        // POST
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RecruiterDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Recruiter created successfully");
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] RecruiterDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok("Recruiter updated successfully");
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Recruiter deleted successfully");
        }
    }
}
