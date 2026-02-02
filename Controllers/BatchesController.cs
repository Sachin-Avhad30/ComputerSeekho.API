using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSeekho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BatchesController : ControllerBase
    {
        private readonly IBatchService _batchService;
        private readonly ILogger<BatchesController> _logger;

        public BatchesController(
            IBatchService batchService,
            ILogger<BatchesController> logger)
        {
            _batchService = batchService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new batch with optional logo
        /// </summary>
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(BatchResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BatchResponseDTO>> CreateBatch(
            [FromForm] BatchCreateRequestDTO dto)
        {
            try
            {
                var result = await _batchService.CreateBatchWithImageAsync(dto);
                return CreatedAtAction(
                    nameof(GetBatchById),
                    new { id = result.BatchId },
                    result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Related entity not found");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating batch");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing batch with optional new logo
        /// </summary>
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(BatchResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BatchResponseDTO>> UpdateBatch(
            int id,
            [FromForm] BatchUpdateRequestDTO dto)
        {
            try
            {
                var result = await _batchService.UpdateBatchWithImageAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Batch or related entity not found: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating batch {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get all batches
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BatchResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BatchResponseDTO>>> GetAllBatches()
        {
            try
            {
                var batches = await _batchService.GetAllBatchesAsync();
                return Ok(batches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all batches");
                return StatusCode(500, new { message = "An error occurred while retrieving batches" });
            }
        }

        /// <summary>
        /// Get all active batches
        /// </summary>
        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<BatchResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BatchResponseDTO>>> GetActiveBatches()
        {
            try
            {
                var batches = await _batchService.GetActiveBatchesAsync();
                return Ok(batches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active batches");
                return StatusCode(500, new { message = "An error occurred while retrieving active batches" });
            }
        }

        /// <summary>
        /// Get batch by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BatchResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BatchResponseDTO>> GetBatchById(int id)
        {
            try
            {
                var batch = await _batchService.GetBatchByIdAsync(id);
                return Ok(batch);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Batch not found: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving batch {Id}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving the batch" });
            }
        }

        /// <summary>
        /// Get batches by course ID (active only)
        /// </summary>
        [HttpGet("course/{courseId}")]
        [ProducesResponseType(typeof(IEnumerable<BatchResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BatchResponseDTO>>> GetBatchesByCourse(int courseId)
        {
            try
            {
                var batches = await _batchService.GetBatchesByCourseIdAsync(courseId);
                return Ok(batches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving batches for course {CourseId}", courseId);
                return StatusCode(500, new { message = "An error occurred while retrieving batches" });
            }
        }

        /// <summary>
        /// Search batches by keyword
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<BatchResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BatchResponseDTO>>> SearchBatches(
            [FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return BadRequest(new { message = "Search keyword is required" });
                }

                var batches = await _batchService.SearchBatchesAsync(keyword);
                return Ok(batches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching batches with keyword: {Keyword}", keyword);
                return StatusCode(500, new { message = "An error occurred while searching batches" });
            }
        }

        /// <summary>
        /// Delete a batch
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBatch(int id)
        {
            try
            {
                await _batchService.DeleteBatchAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Batch not found: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting batch {Id}", id);
                return StatusCode(500, new { message = "An error occurred while deleting the batch" });
            }
        }
    }
}
