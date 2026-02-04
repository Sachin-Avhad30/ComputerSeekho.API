using ComputerSeekho.API.Data;
using ComputerSeekho.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerSeekho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentTypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentTypeController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all active payment types
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<PaymentType>>> GetAllPaymentTypes()
        {
            try
            {
                var paymentTypes = await _context.PaymentTypes
                    .Where(pt => pt.IsActive)
                    .OrderBy(pt => pt.PaymentTypeDesc)
                    .ToListAsync();

                return Ok(paymentTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving payment types", error = ex.Message });
            }
        }

        /// <summary>
        /// Get payment type by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentType>> GetPaymentTypeById(int id)
        {
            try
            {
                var paymentType = await _context.PaymentTypes.FindAsync(id);

                if (paymentType == null)
                {
                    return NotFound(new { message = "Payment type not found" });
                }

                return Ok(paymentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving payment type", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new payment type
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PaymentType>> CreatePaymentType([FromBody] PaymentType paymentType)
        {
            try
            {
                paymentType.CreatedAt = DateTime.Now;
                _context.PaymentTypes.Add(paymentType);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetPaymentTypeById),
                    new { id = paymentType.PaymentTypeId },
                    paymentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating payment type", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing payment type
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentType>> UpdatePaymentType(int id, [FromBody] PaymentType paymentType)
        {
            if (id != paymentType.PaymentTypeId)
            {
                return BadRequest(new { message = "Payment type ID mismatch" });
            }

            try
            {
                var existingPaymentType = await _context.PaymentTypes.FindAsync(id);

                if (existingPaymentType == null)
                {
                    return NotFound(new { message = "Payment type not found" });
                }

                existingPaymentType.PaymentTypeDesc = paymentType.PaymentTypeDesc;
                existingPaymentType.IsActive = paymentType.IsActive;
                existingPaymentType.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(existingPaymentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating payment type", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete (deactivate) a payment type
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePaymentType(int id)
        {
            try
            {
                var paymentType = await _context.PaymentTypes.FindAsync(id);

                if (paymentType == null)
                {
                    return NotFound(new { message = "Payment type not found" });
                }

                // Soft delete - just deactivate
                paymentType.IsActive = false;
                paymentType.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Payment type deactivated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting payment type", error = ex.Message });
            }
        }
    }
}