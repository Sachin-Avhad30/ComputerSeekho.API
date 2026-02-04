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

        public PaymentService(AppDbContext context, ILogger<PaymentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<InstallmentCalculationDTO?> GetInstallmentCalculationAsync(int studentId, int batchId)
        {
            try
            {
                // Get student and batch information
                var student = await _context.StudentMasters.FindAsync(studentId);
                var batch = await _context.Batches.FindAsync(batchId);

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
                // Validate student and batch exist
                var student = await _context.StudentMasters.FindAsync(createPaymentDto.StudentId);
                var batch = await _context.Batches.FindAsync(createPaymentDto.BatchId);
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
                var batch = await _context.Batches.FindAsync(batchId);

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
                var payment = await _context.Payments
                    .Include(p => p.Student)
                    .Include(p => p.Batch)
                    .Include(p => p.PaymentType)
                    .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

                if (payment == null)
                {
                    return null;
                }

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

                return new ReceiptDTO
                {
                    ReceiptId = receipt.ReceiptId,
                    PaymentId = receipt.PaymentId,
                    ReceiptAmount = receipt.ReceiptAmount,
                    ReceiptDate = receipt.ReceiptDate,
                    CreatedAt = receipt.CreatedAt,
                    StudentName = payment.Student?.StudentName ?? "",
                    BatchName = payment.Batch?.BatchName ?? "",
                    PaymentTypeDesc = payment.PaymentType?.PaymentTypeDesc ?? ""
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