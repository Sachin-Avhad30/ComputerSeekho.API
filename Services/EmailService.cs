using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Services.Interfaces;
using System.Net.Mail;
using System.Net;

namespace ComputerSeekho.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly bool _enableSsl;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;

            // Read SMTP settings from configuration
            _smtpHost = _configuration["EmailSettings:SmtpHost"];
            _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            _smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            _smtpPassword = _configuration["EmailSettings:SmtpPassword"];
            _fromEmail = _configuration["EmailSettings:FromEmail"];
            _fromName = _configuration["EmailSettings:FromName"] ?? "ComputerSeekho";
            _enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"] ?? "true");
        }

        public async Task SendEnquiryEmailAsync(EmailRequestDTO dto)
        {
            try
            {
                using (var smtpClient = new SmtpClient(_smtpHost, _smtpPort))
                {
                    smtpClient.EnableSsl = _enableSsl;
                    smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_fromEmail, _fromName),
                        Subject = "New Enquiry Received",
                        Body = $"Name : {dto.Name}\n" +
                               $"Email: {dto.Email}\n\n" +
                               $"Message:\n{dto.Message}",
                        IsBodyHtml = false
                    };

                    // Fixed receiver email
                    mailMessage.To.Add("sheekhocomputer@gmail.com");

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (SmtpException ex)
            {
                throw new Exception($"Failed to send email: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while sending email: {ex.Message}", ex);
            }
        }
    }
}
