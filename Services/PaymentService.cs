using ComputerSeekho.API.Data;
using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Enums;
using ComputerSeekho.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComputerSeekho.API.Services
{
    public class PaymentService : IPaymentService
    {   
        private readonly AppDbContext _context;
        private readonly ILogger<PaymentService> _logger;
        private readonly PdfService _pdfService;
        private readonly IEmailService _emailService;

        public PaymentService(
            AppDbContext context,
            ILogger<PaymentService> logger,
            PdfService pdfService,
            IEmailService emailService)
        {
            _context = context;
            _logger = logger;
            _pdfService = pdfService;
            _emailService = emailService;
        }

        // ... (Keep all existing methods: GetInstallmentCalculationAsync, ProcessPaymentAsync, 
        //      GetStudentPaymentSummaryAsync, GetStudentPaymentsAsync, GetPaymentByIdAsync,
        //      GenerateReceiptAsync - no changes needed)

        public async Task<PaymentPdfDTO?> GetReceiptPdfDataAsync(int receiptId)
        {
            try
            {
                var receipt = await _context.Receipts
                    .Include(r => r.Payment)
                        .ThenInclude(p => p.Student)
                    .Include(r => r.Payment)
                        .ThenInclude(p => p.Batch)
                            .ThenInclude(b => b.Course)
                    .Include(r => r.Payment)
                        .ThenInclude(p => p.PaymentType)
                    .FirstOrDefaultAsync(r => r.ReceiptId == receiptId);

                if (receipt == null)
                {
                    return null;
                }

                var payment = receipt.Payment;
                var student = payment?.Student;
                var batch = payment?.Batch;
                var course = batch?.Course;
                var paymentType = payment?.PaymentType;

                // ✅ Get ALL payments for this student in this batch
                var allPayments = await _context.Payments
                    .Include(p => p.PaymentType)
                    .Where(p => p.StudentId == payment.StudentId && p.BatchId == payment.BatchId)
                    .OrderBy(p => p.PaymentDate)
                    .ToListAsync();

                // Calculate totals
                decimal totalPaid = allPayments.Sum(p => p.PaymentAmount);
                decimal totalFees = batch?.CourseFees ?? 0;
                decimal remainingBalance = totalFees - totalPaid;

                // Map to PreviousPaymentDTO
                var previousPayments = allPayments.Select(p => new PreviousPaymentDTO
                {
                    PaymentId = p.PaymentId,
                    PaymentAmount = p.PaymentAmount,
                    PaymentDate = p.PaymentDate,
                    PaymentTypeDesc = p.PaymentType?.PaymentTypeDesc ?? ""
                }).ToList();

                return new PaymentPdfDTO
                {
                    ReceiptId = receipt.ReceiptId,
                    StudentName = student?.StudentName ?? "",
                    StudentMobile = student?.StudentMobile.ToString() ?? "",
                    StudentAddress = student?.StudentAddress ?? "",
                    StudentEmail = student?.StudentUsername ?? "", // USERNAME IS EMAIL!
                    CourseName = course?.CourseName ?? "",
                    BatchName = batch?.BatchName ?? "",
                    Amount = payment?.PaymentAmount ?? 0,
                    PaymentDate = payment?.PaymentDate ?? DateTime.Now,
                    PaymentType = paymentType?.PaymentTypeDesc ?? "",
                    ReceiptAmount = receipt.ReceiptAmount,
                    ReceiptDate = receipt.ReceiptDate,
                    TransactionReference = payment?.TransactionReference,

                    // ✅ Payment Summary
                    TotalCourseFees = totalFees,
                    TotalPaidTillNow = totalPaid,
                    RemainingBalance = remainingBalance,

                    // ✅ All Previous Payments
                    AllPreviousPayments = previousPayments
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting receipt PDF data");
                throw;
            }
        }
        public async Task<string> GeneratePdfAndSendEmailAsync(int receiptId)
        {
            try
            {
                // Get receipt data
                var pdfData = await GetReceiptPdfDataAsync(receiptId);

                if (pdfData == null)
                {
                    throw new KeyNotFoundException($"Receipt with ID {receiptId} not found");
                }

                // Generate PDF
                byte[] pdfBytes = _pdfService.GenerateReceiptPdf(pdfData);

                // Send email to student's username (which IS the email)
                await _emailService.SendReceiptPdfEmailAsync(pdfData.StudentEmail, pdfBytes);

                return $"Receipt PDF sent to email: {pdfData.StudentEmail}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF and sending email");
                throw;
            }
        }

        public async Task<InstallmentCalculationDTO?> GetInstallmentCalculationAsync(int studentId, int batchId)
        {
            try
            {
                var student = await _context.StudentMasters.FindAsync(studentId);
                var batch = await _context.Batches
                    .Include(b => b.Course)
                    .FirstOrDefaultAsync(b => b.BatchId == batchId);

                if (student == null || batch == null)
                {
                    return null;
                }

                // Get all payments for this student in this batch
                var payments = await _context.Payments
                    .Include(p => p.PaymentType)
                    .Where(p => p.StudentId == studentId && p.BatchId == batchId)
                    .OrderBy(p => p.PaymentDate)
                    .ToListAsync();

                decimal totalPaid = payments.Sum(p => p.PaymentAmount);
                decimal totalFees = batch.CourseFees ?? 0;
                decimal remainingBalance = totalFees - totalPaid;

                var previousPayments = payments.Select(p => new PreviousPaymentDTO
                {
                    PaymentId = p.PaymentId,
                    PaymentAmount = p.PaymentAmount,
                    PaymentDate = p.PaymentDate,
                    PaymentTypeDesc = p.PaymentType?.PaymentTypeDesc ?? ""
                }).ToList();

                return new InstallmentCalculationDTO
                {
                    StudentId = studentId,
                    BatchId = batchId,
                    TotalCourseFees = totalFees,
                    TotalPaid = totalPaid,
                    RemainingBalance = remainingBalance,
                    InstallmentsPaid = payments.Count,
                    PreviousPayments = previousPayments
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating installments for student {StudentId}", studentId);
                throw;
            }
        }

        public async Task<PaymentDTO?> ProcessPaymentAsync(CreatePaymentDTO createPaymentDto)
        {
            try
            {
                var student = await _context.StudentMasters.FindAsync(createPaymentDto.StudentId);
                var batch = await _context.Batches
                    .Include(b => b.Course)
                    .FirstOrDefaultAsync(b => b.BatchId == createPaymentDto.BatchId);
                var paymentType = await _context.PaymentTypes.FindAsync(createPaymentDto.PaymentTypeId);

                if (student == null)
                    throw new KeyNotFoundException($"Student with ID {createPaymentDto.StudentId} not found");
                if (batch == null)
                    throw new KeyNotFoundException($"Batch with ID {createPaymentDto.BatchId} not found");
                if (paymentType == null)
                    throw new KeyNotFoundException($"Payment type with ID {createPaymentDto.PaymentTypeId} not found");

                // Create new payment
                var payment = new Payment
                {
                    StudentId = createPaymentDto.StudentId,
                    BatchId = createPaymentDto.BatchId,
                    PaymentTypeId = createPaymentDto.PaymentTypeId,
                    PaymentAmount = createPaymentDto.PaymentAmount,
                    PaymentDate = DateTime.Now,
                    TransactionReference = createPaymentDto.TransactionReference,
                    Remarks = createPaymentDto.Remarks,
                    PaymentStatus = "COMPLETED",
                    CreatedAt = DateTime.Now
                };

                _context.Payments.Add(payment);

                // Update student registration status to Active after first payment
                if (student.RegistrationStatus == RegistrationStatus.PaymentPending)
                {
                    student.RegistrationStatus = RegistrationStatus.Active;
                }

                // Check if payment is complete
                var totalPaid = await _context.Payments
                    .Where(p => p.StudentId == createPaymentDto.StudentId && p.BatchId == createPaymentDto.BatchId)
                    .SumAsync(p => p.PaymentAmount);

                totalPaid += createPaymentDto.PaymentAmount; // Include current payment

                if (batch.CourseFees.HasValue && totalPaid >= batch.CourseFees.Value)
                {
                    student.RegistrationStatus = RegistrationStatus.FullyPaid;
                }

                await _context.SaveChangesAsync();

                // Return payment DTO
                return new PaymentDTO
                {
                    PaymentId = payment.PaymentId,
                    StudentId = payment.StudentId,
                    StudentName = student.StudentName,
                    BatchId = payment.BatchId,
                    BatchName = batch.BatchName,
                    PaymentTypeId = payment.PaymentTypeId,
                    PaymentTypeDesc = paymentType.PaymentTypeDesc,
                    PaymentAmount = payment.PaymentAmount,
                    PaymentDate = payment.PaymentDate,
                    TransactionReference = payment.TransactionReference,
                    PaymentStatus = payment.PaymentStatus,
                    Remarks = payment.Remarks,
                    CreatedAt = payment.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                throw;
            }
        }

        public async Task<StudentPaymentSummaryDTO?> GetStudentPaymentSummaryAsync(int studentId, int batchId)
        {
            try
            {
                var student = await _context.StudentMasters.FindAsync(studentId);
                var batch = await _context.Batches
                    .Include(b => b.Course)
                    .FirstOrDefaultAsync(b => b.BatchId == batchId);

                if (student == null || batch == null)
                {
                    return null;
                }

                var payments = await _context.Payments
                    .Include(p => p.PaymentType)
                    .Where(p => p.StudentId == studentId && p.BatchId == batchId)
                    .OrderByDescending(p => p.PaymentDate)
                    .ToListAsync();

                decimal totalPaid = payments.Sum(p => p.PaymentAmount);
                decimal totalFees = batch.CourseFees ?? 0;
                decimal remainingBalance = totalFees - totalPaid;

                var paymentHistory = payments.Select(p => new PaymentDTO
                {
                    PaymentId = p.PaymentId,
                    StudentId = p.StudentId,
                    StudentName = student.StudentName,
                    BatchId = p.BatchId,
                    BatchName = batch.BatchName,
                    PaymentTypeId = p.PaymentTypeId,
                    PaymentTypeDesc = p.PaymentType?.PaymentTypeDesc ?? "",
                    PaymentAmount = p.PaymentAmount,
                    PaymentDate = p.PaymentDate,
                    TransactionReference = p.TransactionReference,
                    PaymentStatus = p.PaymentStatus,
                    Remarks = p.Remarks,
                    CreatedAt = p.CreatedAt
                }).ToList();

                return new StudentPaymentSummaryDTO
                {
                    StudentId = studentId,
                    StudentName = student.StudentName,
                    BatchId = batchId,
                    BatchName = batch.BatchName,
                    TotalCourseFees = totalFees,
                    TotalPaid = totalPaid,
                    RemainingBalance = remainingBalance,
                    InstallmentsPaid = payments.Count,
                    PaymentHistory = paymentHistory
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student payment summary");
                throw;
            }
        }

        public async Task<List<PaymentDTO>> GetStudentPaymentsAsync(int studentId)
        {
            try
            {
                var payments = await _context.Payments
                    .Include(p => p.Student)
                    .Include(p => p.Batch)
                    .Include(p => p.PaymentType)
                    .Where(p => p.StudentId == studentId)
                    .OrderByDescending(p => p.PaymentDate)
                    .ToListAsync();

                return payments.Select(p => new PaymentDTO
                {
                    PaymentId = p.PaymentId,
                    StudentId = p.StudentId,
                    StudentName = p.Student?.StudentName ?? "",
                    BatchId = p.BatchId,
                    BatchName = p.Batch?.BatchName ?? "",
                    PaymentTypeId = p.PaymentTypeId,
                    PaymentTypeDesc = p.PaymentType?.PaymentTypeDesc ?? "",
                    PaymentAmount = p.PaymentAmount,
                    PaymentDate = p.PaymentDate,
                    TransactionReference = p.TransactionReference,
                    PaymentStatus = p.PaymentStatus,
                    Remarks = p.Remarks,
                    CreatedAt = p.CreatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student payments");
                throw;
            }
        }

        public async Task<PaymentDTO?> GetPaymentByIdAsync(int paymentId)
        {
            try
            {
                var payment = await _context.Payments
                    .Include(p => p.Student)
                    .Include(p => p.Batch)
                    .Include(p => p.PaymentType)
                    .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

                if (payment == null)
                {
                    return null;
                }

                return new PaymentDTO
                {
                    PaymentId = payment.PaymentId,
                    StudentId = payment.StudentId,
                    StudentName = payment.Student?.StudentName ?? "",
                    BatchId = payment.BatchId,
                    BatchName = payment.Batch?.BatchName ?? "",
                    PaymentTypeId = payment.PaymentTypeId,
                    PaymentTypeDesc = payment.PaymentType?.PaymentTypeDesc ?? "",
                    PaymentAmount = payment.PaymentAmount,
                    PaymentDate = payment.PaymentDate,
                    TransactionReference = payment.TransactionReference,
                    PaymentStatus = payment.PaymentStatus,
                    Remarks = payment.Remarks,
                    CreatedAt = payment.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment by ID");
                throw;
            }
        }

        public async Task<ReceiptDTO?> GenerateReceiptAsync(int paymentId)
        {
            try
            {
                // Get the current payment with all related data
                var payment = await _context.Payments
                    .Include(p => p.Student)
                    .Include(p => p.Batch)
                        .ThenInclude(b => b.Course)
                    .Include(p => p.PaymentType)
                    .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

                if (payment == null)
                {
                    return null;
                }

                // Get ALL previous payments for this student in this batch (including current one)
                var allPayments = await _context.Payments
                    .Include(p => p.PaymentType)
                    .Where(p => p.StudentId == payment.StudentId && p.BatchId == payment.BatchId)
                    .OrderBy(p => p.PaymentDate)
                    .ToListAsync();

                // Calculate totals
                decimal totalPaid = allPayments.Sum(p => p.PaymentAmount);
                decimal totalFees = payment.Batch?.CourseFees ?? 0;
                decimal remainingBalance = totalFees - totalPaid;

                // Map all payments to PreviousPaymentDTO
                var previousPayments = allPayments.Select(p => new PreviousPaymentDTO
                {
                    PaymentId = p.PaymentId,
                    PaymentAmount = p.PaymentAmount,
                    PaymentDate = p.PaymentDate,
                    PaymentTypeDesc = p.PaymentType?.PaymentTypeDesc ?? ""
                }).ToList();

                // Create receipt
                var receipt = new Receipt
                {
                    PaymentId = paymentId,
                    ReceiptAmount = payment.PaymentAmount,
                    ReceiptDate = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                _context.Receipts.Add(receipt);
                await _context.SaveChangesAsync();

                // Return complete receipt DTO
                return new ReceiptDTO
                {
                    ReceiptId = receipt.ReceiptId,
                    PaymentId = receipt.PaymentId,
                    ReceiptAmount = receipt.ReceiptAmount,
                    ReceiptDate = receipt.ReceiptDate,
                    CreatedAt = receipt.CreatedAt,

                    // Student & Batch Info
                    StudentName = payment.Student?.StudentName ?? "",
                    StudentMobile = payment.Student?.StudentMobile ?? 0,
                    BatchName = payment.Batch?.BatchName ?? "",
                    CourseName = payment.Batch?.Course?.CourseName ?? "",

                    // Payment Summary
                    TotalCourseFees = totalFees,
                    TotalPaidTillNow = totalPaid,
                    RemainingBalance = remainingBalance,

                    // Current Payment Info
                    PaymentTypeDesc = payment.PaymentType?.PaymentTypeDesc ?? "",
                    TransactionReference = payment.TransactionReference,

                    // All Previous Payments (including this one)
                    AllPreviousPayments = previousPayments
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating receipt");
                throw;
            }
        }
    }
}