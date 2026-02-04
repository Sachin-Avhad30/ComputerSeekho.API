using ComputerSeekho.API.Repositories;
using ComputerSeekho.Application.Services.Interfaces;
using System.Security.Claims;

namespace ComputerSeekho.API.Middleware
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtAuthenticationMiddleware> _logger;

        public JwtAuthenticationMiddleware(
            RequestDelegate next,
            ILogger<JwtAuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IJwtService jwtService, IStaffRepository staffRepository)
        {
            try
            {
                // Extract JWT from Authorization header
                var token = ParseJwt(context.Request);

                if (!string.IsNullOrEmpty(token) && jwtService.ValidateJwtToken(token))
                {
                    var username = jwtService.GetUsernameFromToken(token);

                    if (!string.IsNullOrEmpty(username))
                    {
                        // Load user details
                        var staff = await staffRepository.GetByUsernameAsync(username);

                        if (staff != null)
                        {
                            // Set user identity with claims
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, staff.StaffId.ToString()),
                                new Claim(ClaimTypes.Name, staff.StaffUsername),
                                new Claim(ClaimTypes.Email, staff.StaffEmail),
                                new Claim("StaffName", staff.StaffName),
                                new Claim(ClaimTypes.Role, staff.StaffRole.Equals("admin", StringComparison.OrdinalIgnoreCase)
                                    ? "ROLE_ADMIN"
                                    : "ROLE_TEACHING")
                            };

                            var identity = new ClaimsIdentity(claims, "jwt");
                            var principal = new ClaimsPrincipal(identity);

                            context.User = principal;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot set user authentication: {Message}", ex.Message);
            }

            await _next(context);
        }

        /// <summary>
        /// Extract JWT from Authorization header
        /// </summary>
        private string? ParseJwt(HttpRequest request)
        {
            var headerAuth = request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrEmpty(headerAuth) && headerAuth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return headerAuth.Substring(7);
            }

            return null;
        }
    }
}