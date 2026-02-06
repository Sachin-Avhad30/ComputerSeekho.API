using ComputerSeekho.API.DTOs.Request;
using ComputerSeekho.API.DTOs.Response;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Repositories;
using ComputerSeekho.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using BCrypt.Net;
using ComputerSeekho.API.Enum;

namespace ComputerSeekho.Application.Services
{
    public class StaffAuthService : IStaffAuthService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IJwtService _jwtService;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<StaffAuthService> _logger;

        public StaffAuthService(
            IStaffRepository staffRepository,
            IJwtService jwtService,
            IFileStorageService fileStorageService,
            ILogger<StaffAuthService> logger)
        {
            _staffRepository = staffRepository;
            _jwtService = jwtService;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task<StaffJwtResponse> AuthenticateStaffAsync(StaffLoginRequest loginRequest)
        {
            // Find staff by username
            var staff = await _staffRepository.GetByUsernameAsync(loginRequest.StaffUsername);

            if (staff == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Verify password using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(loginRequest.StaffPassword, staff.StaffPassword))
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Check if staff is active
            if (!staff.IsActive)
            {
                throw new UnauthorizedAccessException("Staff account is deactivated");
            }

            // Generate JWT token
            var token = _jwtService.GenerateJwtToken(staff);

            // Return JWT response
            return new StaffJwtResponse(
                token,
                staff.StaffId,
                staff.StaffUsername,
                staff.StaffEmail,
                staff.StaffName,
                staff.StaffRole
            );
        }

        public async Task<MessageResponse> RegisterStaffWithImageAsync(
            IFormFile? staffImage,
            string staffName,
            string staffMobile,
            string staffEmail,
            string staffUsername,
            string staffPassword,
            string staffRole,
            string? staffDesignation,
            string? staffBio)
        {
            // Check if username already exists
            if (await _staffRepository.ExistsByUsernameAsync(staffUsername))
            {
                return new MessageResponse("Error: Username is already taken!");
            }

            // Check if email already exists
            if (await _staffRepository.ExistsByEmailAsync(staffEmail))
            {
                return new MessageResponse("Error: Email is already in use!");
            }

            // Validate staff role
            var role = staffRole.ToLower();
            if (role != "non-teaching" && role != "teaching")
            {
                return new MessageResponse("Error: Invalid staff role! Use 'non-teaching' or 'teaching'");
            }

            // Handle image upload
            string? photoUrl = null;
            if (staffImage != null && staffImage.Length > 0)
            {
                try
                {
                    // Validate file size (max 5MB)
                    if (staffImage.Length > 5 * 1024 * 1024)
                    {
                        return new MessageResponse("Error: File size exceeds 5MB limit!");
                    }

                    // Store image using FileStorageService
                    // Using "Staff" as UploadType (changed from Faculty as per your requirement)
                    photoUrl = await _fileStorageService.StoreImageAsync(
                        staffImage,
                        UploadType.Faculty, // This maps to "faculty" folder, change to Staff if you update enum
                        staffUsername
                    );

                    _logger.LogInformation("Staff image uploaded successfully: {PhotoUrl}", photoUrl);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to upload staff image");
                    return new MessageResponse($"Error: Failed to upload image - {ex.Message}");
                }
            }

            // Create new staff account
            var staff = new Staff
            {
                StaffUsername = staffUsername,
                StaffEmail = staffEmail,
                StaffPassword = BCrypt.Net.BCrypt.HashPassword(staffPassword), // Hash password using BCrypt
                StaffName = staffName,
                PhotoUrl = photoUrl,
                StaffMobile = staffMobile,
                StaffRole = role,
                StaffBio = staffBio,
                StaffDesignation = staffDesignation,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save to database
            await _staffRepository.AddAsync(staff);

            _logger.LogInformation("Staff registered successfully: {StaffUsername}", staffUsername);

            return new MessageResponse("Staff registered successfully!");
        }

        public async Task<StaffJwtResponse> AuthenticateStaffWithGoogleAsync(string email)
        {
            var staff = await _staffRepository.GetByEmailAsync(email);

            if (staff == null)
            {
                throw new Exception($"Staff not found with email: {email}");
            }

            // Generate JWT token
            var token = _jwtService.GenerateTokenFromUsername(staff.StaffUsername);

            return new StaffJwtResponse(
                token,
                staff.StaffId,
                staff.StaffUsername,
                staff.StaffEmail,
                staff.StaffName,
                staff.StaffRole
            );
        }
    }
}