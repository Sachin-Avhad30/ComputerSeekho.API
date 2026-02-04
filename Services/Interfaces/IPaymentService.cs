using ComputerSeekho.API.DTOs;

namespace ComputerSeekho.API.Services.Interfaces
{
    public interface IPaymentService
    {
        // Get installment calculation for a student
        Task<InstallmentCalculationDTO?> GetInstallmentCalculationAsync(int studentId, int batchId);

        // Process a new payment
        Task<PaymentDTO?> ProcessPaymentAsync(CreatePaymentDTO createPaymentDto);

        // Get payment summary for a student
        Task<StudentPaymentSummaryDTO?> GetStudentPaymentSummaryAsync(int studentId, int batchId);

        // Get all payments for a student
        Task<List<PaymentDTO>> GetStudentPaymentsAsync(int studentId);

        // Get payment by ID
        Task<PaymentDTO?> GetPaymentByIdAsync(int paymentId);

        // Generate receipt for a payment
        Task<ReceiptDTO?> GenerateReceiptAsync(int paymentId);
    }
}