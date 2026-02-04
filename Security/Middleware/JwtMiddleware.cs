using ComputerSeekho.API.Repositories.Interfaces;
using ComputerSeekho.API.Security.JWT;

namespace ComputerSeekho.API.Security.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IStaffRepository staffRepository, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var staffId = jwtUtils.ValidateJwtToken(token);

            if (staffId != null)
            {
                // Attach staff to context on successful jwt validation
                context.Items["Staff"] = await staffRepository.GetByIdAsync(staffId.Value);
            }

            await _next(context);
        }
    }
}
