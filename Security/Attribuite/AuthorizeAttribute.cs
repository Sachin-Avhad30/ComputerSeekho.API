using ComputerSeekho.API.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSeekho.API.Security.Attribuite
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public AuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var staff = (Staff)context.HttpContext.Items["Staff"];

            if (staff == null)
            {
                // Not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

            // Check if roles are specified and if staff has required role
            if (_roles != null && _roles.Length > 0)
            {
                if (!_roles.Contains(staff.StaffRole, StringComparer.OrdinalIgnoreCase))
                {
                    // Staff doesn't have required role
                    context.Result = new JsonResult(new { message = "Forbidden - Insufficient permissions" })
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                }
            }
        }
    }
}
