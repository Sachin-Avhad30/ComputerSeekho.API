using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSeekho.API.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        /// <summary>
        /// Get installment calculation for a student in a batch
        /// Shows: Total Fees, Amount Paid, Remaining Balance, Previous Payments
        /// </summary>
        [HttpGet("installment-calculation")]
        [ProducesResponseType(typeof(InstallmentCalculationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InstallmentCalculationDTO>> GetInstallmentCalculation(
            [FromQuery] int studentId,
            [FromQuery] int batchId)
        {
            try
            {
                var calculation = await _paymentService.GetInstallmentCalculationAsync(studentId, batchId);

                if (calculation == null)
                {
                    return NotFound(new { message = "Student or batch not found" });
                }

                return Ok(calculation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating installments for student {StudentId} in batch {BatchId}",
                    studentId, batchId);
                return StatusCode(500, new { message = "Error calculating installments" });
            }
        }

        /// <summary>
        /// Process a new payment for a student
        /// IMPORTANT: First call GET /installment-calculation to see remaining balance!
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PaymentDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaymentDTO>> ProcessPayment([FromBody] CreatePaymentDTO createPaymentDto)
        {
            try
            {
                var payment = await _paymentService.ProcessPaymentAsync(createPaymentDto);

                if (payment == null)
                {
                    return BadRequest(new { message = "Failed to process payment" });
                }

                return CreatedAtAction(
                    nameof(GetPaymentById),
                    new { id = payment.PaymentId },
                    payment);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Entity not found while processing payment");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get payment summary for a student in a batch
        /// Shows complete payment history with totals
        /// </summary>
        [HttpGet("student-summary")]
        [ProducesResponseType(typeof(StudentPaymentSummaryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentPaymentSummaryDTO>> GetStudentPaymentSummary(
            [FromQuery] int studentId,
            [FromQuery] int batchId)
        {
            try
            {
                var summary = await _paymentService.GetStudentPaymentSummaryAsync(studentId, batchId);

                if (summary == null)
                {
                    return NotFound(new { message = "Student or batch not found" });
                }

                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment summary for student {StudentId}", studentId);
                return StatusCode(500, new { message = "Error retrieving payment summary" });
            }
        }

        /// <summary>
        /// Get all payments for a student (across all batches)
        /// </summary>
        [HttpGet("student/{studentId}")]
        [ProducesResponseType(typeof(List<PaymentDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PaymentDTO>>> GetStudentPayments(int studentId)
        {
            try
            {
                var payments = await _paymentService.GetStudentPaymentsAsync(studentId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payments for student {StudentId}", studentId);
                return StatusCode(500, new { message = "Error retrieving payments" });
            }
        }

        /// <summary>
        /// Get payment by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PaymentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentDTO>> GetPaymentById(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);

                if (payment == null)
                {
                    return NotFound(new { message = "Payment not found" });
                }

                return Ok(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment {PaymentId}", id);
                return StatusCode(500, new { message = "Error retrieving payment" });
            }
        }

        /// <summary>
        /// Generate receipt for a payment
        /// Receipt includes: Student details, Batch/Course info, All previous payments, Balance summary
        /// You can generate receipt multiple times for the same payment
        /// </summary>
        [HttpPost("{paymentId}/receipt")]
        [ProducesResponseType(typeof(ReceiptDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReceiptDTO>> GenerateReceipt(int paymentId)
        {
            try
            {
                var receipt = await _paymentService.GenerateReceiptAsync(paymentId);

                if (receipt == null)
                {
                    return NotFound(new { message = "Payment not found" });
                }

                return CreatedAtAction(
                    nameof(GetPaymentById),
                    new { id = paymentId },
                    receipt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating receipt for payment {PaymentId}", paymentId);
                return StatusCode(500, new { message = "Error generating receipt" });
            }
        }

        /// <summary>
        /// Generate receipt PDF and send it to student's email
        /// </summary>
        [HttpGet("receipt/{receiptId}/email")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> SendReceiptEmail(int receiptId)
        {
            try
            {
                var result = await _paymentService.GeneratePdfAndSendEmailAsync(receiptId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Receipt not found: {ReceiptId}", receiptId);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending receipt email for {ReceiptId}", receiptId);
                return StatusCode(500, new { message = "Error sending receipt email: " + ex.Message });
            }
        }
    }
}