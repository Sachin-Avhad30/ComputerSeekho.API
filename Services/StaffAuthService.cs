using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Repositories.Interfaces;
using ComputerSeekho.API.Security.JWT;
using ComputerSeekho.API.Services.Interfaces;
using ComputerSeekho.Application.Services.Interfaces;
using Org.BouncyCastle.Crypto.Generators;

namespace ComputerSeekho.API.Services
{
    public class StaffAuthService : IStaffAuthService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IJwtUtils _jwtUtils;
        private readonly IFileStorageService _fileStorageService;

        public StaffAuthService(
            IStaffRepository staffRepository,
            IJwtUtils jwtUtils,
            IFileStorageService fileStorageService)
        {
            _staffRepository = staffRepository;
            _jwtUtils = jwtUtils;
            _fileStorageService = fileStorageService;
        }

        public async Task<StaffJwtResponse> AuthenticateStaffAsync(StaffLoginRequest loginRequest)
        {
            // Get staff by username
            var staff = await _staffRepository.GetByUsernameAsync(loginRequest.StaffUsername);

            // Validate staff exists and password is correct
            if (staff == null || !VerifyPasswordHash(loginRequest.StaffPassword, staff.StaffPassword))
            {
                throw new Exception("Invalid username or password");
            }

            // Check if staff is active
            if (!staff.IsActive)
            {
                throw new Exception("Account is inactive. Please contact administrator.");
            }

            // Generate JWT token
            var token = _jwtUtils.GenerateJwtToken(staff);

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

        public async Task<MessageResponse> RegisterStaffWithImageAsync(StaffSignupRequest signupRequest)
        {
            // Check if username already exists
            if (await _staffRepository.ExistsByUsernameAsync(signupRequest.StaffUsername))
            {
                return new MessageResponse("Error: Username is already taken!");
            }

            // Check if email already exists
            if (await _staffRepository.ExistsByEmailAsync(signupRequest.StaffEmail))
            {
                return new MessageResponse("Error: Email is already in use!");
            }

            // Validate staff role
            var role = signupRequest.StaffRole.ToLower();
            if (role != "teaching" && role != "non-teaching" && role != "admin")
            {
                return new MessageResponse("Error: Invalid staff role! Use 'teaching', 'admin', or 'non-teaching'");
            }

            // Handle image upload
            string photoUrl = null;
            if (signupRequest.StaffImage != null && signupRequest.StaffImage.Length > 0)
            {
                try
                {
                    photoUrl = await _fileStorageService.StoreImageAsync(
                        signupRequest.StaffImage,
                        Enum.UploadType.Faculty,
                        signupRequest.StaffUsername
                    );
                }
                catch (Exception ex)
                {
                    return new MessageResponse($"Error: Failed to upload image - {ex.Message}");
                }
            }

            // Create new staff account
            var staff = new Staff
            {
                StaffUsername = signupRequest.StaffUsername,
                StaffEmail = signupRequest.StaffEmail,
                StaffPassword = HashPassword(signupRequest.StaffPassword),
                StaffName = signupRequest.StaffName,
                PhotoUrl = photoUrl,
                StaffMobile = signupRequest.StaffMobile,
                StaffRole = role,
                StaffBio = signupRequest.StaffBio,
                StaffDesignation = signupRequest.StaffDesignation,
                IsActive = true
            };

            await _staffRepository.CreateAsync(staff);

            return new MessageResponse("Staff registered successfully!");
        }

        public async Task<List<StaffInfoResponse>> GetFacultyListAsync()
        {
            var facultyList = await _staffRepository.GetByStaffRoleAsync("teaching");

            return facultyList.Select(staff => new StaffInfoResponse
            {
                StaffId = staff.StaffId,
                StaffName = staff.StaffName,
                PhotoUrl = staff.PhotoUrl,
                StaffMobile = staff.StaffMobile,
                StaffEmail = staff.StaffEmail,
                StaffUsername = staff.StaffUsername,
                StaffRole = staff.StaffRole,
                IsActive = staff.IsActive,
                StaffBio = staff.StaffBio,
                StaffDesignation = staff.StaffDesignation
            }).ToList();
        }

        public async Task<List<StaffInfoResponse>> GetAllStaffAsync()
        {
            var staffList = await _staffRepository.GetAllAsync();

            return staffList.Select(staff => new StaffInfoResponse
            {
                StaffId = staff.StaffId,
                StaffName = staff.StaffName,
                PhotoUrl = staff.PhotoUrl,
                StaffMobile = staff.StaffMobile,
                StaffEmail = staff.StaffEmail,
                StaffUsername = staff.StaffUsername,
                StaffRole = staff.StaffRole,
                IsActive = staff.IsActive,
                StaffBio = staff.StaffBio,
                StaffDesignation = staff.StaffDesignation
            }).ToList();
        }

        public async Task<StaffInfoResponse> GetStaffByIdAsync(int staffId)
        {
            var staff = await _staffRepository.GetByIdAsync(staffId);

            if (staff == null)
                return null;

            return new StaffInfoResponse
            {
                StaffId = staff.StaffId,
                StaffName = staff.StaffName,
                PhotoUrl = staff.PhotoUrl,
                StaffMobile = staff.StaffMobile,
                StaffEmail = staff.StaffEmail,
                StaffUsername = staff.StaffUsername,
                StaffRole = staff.StaffRole,
                IsActive = staff.IsActive,
                StaffBio = staff.StaffBio,
                StaffDesignation = staff.StaffDesignation
            };
        }

        // Password hashing methods
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPasswordHash(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
