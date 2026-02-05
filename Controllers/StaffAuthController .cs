using ComputerSeekho.API.DTOs.Request;
using ComputerSeekho.API.DTOs.Response;
using ComputerSeekho.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ComputerSeekho.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class StaffAuthController : ControllerBase
    {
        private readonly IStaffAuthService _staffAuthService;
        private readonly ILogger<StaffAuthController> _logger;

        public StaffAuthController(
            IStaffAuthService staffAuthService,
            ILogger<StaffAuthController> logger)
        {
            _staffAuthService = staffAuthService;
            _logger = logger;
        }

        /// <summary>
        /// Staff Login endpoint
        /// POST /api/auth/login
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateStaff([FromBody] StaffLoginRequest loginRequest)
        {
            try
            {
                var jwtResponse = await _staffAuthService.AuthenticateStaffAsync(loginRequest);
                return Ok(jwtResponse);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Login failed: {Message}", ex.Message);
                return Unauthorized(new MessageResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during authentication");
                return StatusCode(500, new MessageResponse("An error occurred during authentication"));
            }
        }

        /// <summary>
        /// Staff Signup/Register endpoint with IMAGE UPLOAD
        /// POST /api/auth/signup
        /// Accepts multipart/form-data with:
        /// - staffImage (file, optional)
        /// - All other staff fields as form data
        /// </summary>
        [HttpPost("signup")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> RegisterStaff(
            [FromForm] StaffSignupRequest request)
        {
            try
            {
                var messageResponse = await _staffAuthService.RegisterStaffWithImageAsync(
                    request.StaffImage,
                    request.StaffName,
                    request.StaffMobile,
                    request.StaffEmail,
                    request.StaffUsername,
                    request.StaffPassword,
                    request.StaffRole,
                    request.StaffDesignation,
                    request.StaffBio
                );

                if (messageResponse.Message.StartsWith("Error"))
                {
                    return BadRequest(messageResponse);
                }

                return Ok(messageResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during staff registration");
                return BadRequest(new MessageResponse($"Error: {ex.Message}"));
            }
        }









        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            _logger.LogInformation("Initiating Google OAuth login");

            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GetOAuth2User)),
                AllowRefresh = true
            };

            _logger.LogInformation("Redirect URI: {RedirectUri}", properties.RedirectUri);

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// OAuth2 Google Authentication Success Handler
        /// GET /api/auth/oauth2/user
        /// </summary>
        [HttpGet("oauth2/user")]
        public async Task<IActionResult> GetOAuth2User()
        {
            try
            {
                _logger.LogInformation("OAuth2 callback received");

                // Authenticate using Cookie scheme
                var authenticateResult = await HttpContext.AuthenticateAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);

                if (!authenticateResult.Succeeded)
                {
                    _logger.LogWarning("Cookie authentication failed");
                    return Redirect("http://localhost:5173/admin/login?error=auth_failed");
                }

                _logger.LogInformation("Cookie authentication succeeded");

                // Log all claims for debugging
                var claims = authenticateResult.Principal?.Claims.ToList();
                if (claims != null)
                {
                    foreach (var claim in claims)
                    {
                        _logger.LogInformation("Claim: {Type} = {Value}", claim.Type, claim.Value);
                    }
                }

                // Get email claim
                var emailClaim = authenticateResult.Principal?.Claims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.Email ||
                    c.Type == "email" ||
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

                if (emailClaim == null)
                {
                    _logger.LogWarning("Email claim not found in OAuth response");
                    return Redirect("http://localhost:5173/admin/login?error=no_email");
                }

                string email = emailClaim.Value;
                _logger.LogInformation("Google OAuth successful for email: {Email}", email);

                // Generate JWT using Google email
                var jwtResponse = await _staffAuthService.AuthenticateStaffWithGoogleAsync(email);

                // Sign out of cookie auth (we only need the JWT now)
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                _logger.LogInformation("JWT generated successfully, redirecting to frontend");

                // Redirect to React with token
                return Redirect($"http://localhost:5173/oauth-success?token={jwtResponse.Token}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OAuth2 authentication failed: {Message}", ex.Message);
                _logger.LogError("Stack trace: {StackTrace}", ex.StackTrace);
                return Redirect($"http://localhost:5173/admin/login?error=server_error&message={Uri.EscapeDataString(ex.Message)}");
            }
        }
        /// <summary>
        /// Test endpoint to check if API is working
        /// GET /api/auth/test
        /// </summary>
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new MessageResponse("Staff Auth API is working!"));
        }
    }
}