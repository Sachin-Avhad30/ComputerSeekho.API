using ComputerSeekho.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace ComputerSeekho.API.Controllers
{
    [ApiController]
    [Route("api/albums")]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _service;

        public AlbumController(IAlbumService service)
        {
            _service = service;
        }


        [HttpGet("{albumName}")]
        public async Task<IActionResult> GetAllImages(string albumName)
        {
            var result = await _service.GetAllImagesAsync(albumName);
            return Ok(result);
        }

        [HttpPost("upload-excel")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            await _service.UploadExcelAsync(file);

            return Ok("Excel uploaded & data inserted successfully");
        }
    }
}
