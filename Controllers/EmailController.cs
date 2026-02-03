using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSeekho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailController> _logger;

        public EmailController(
            IEmailService emailService,
            ILogger<EmailController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// Send an enquiry email
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> SendEmail([FromBody] EmailRequestDTO dto)
        {
            try
            {
                await _emailService.SendEnquiryEmailAsync(dto);
                return Ok("Email sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending enquiry email from {Email}", dto?.Email);
                return StatusCode(500, new { message = "Failed to send email. Please try again later." });
            }
        }
    }
}
