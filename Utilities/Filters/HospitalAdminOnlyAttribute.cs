using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Utilities.Filters
{
    public class HospitalAdminOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            var roleClaim = user.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(roleClaim) 
            || !Enum.TryParse<RoleType>(roleClaim, out var role) 
            || (role != RoleType.Hospital && role != RoleType.Admin))
            {
                context.Result = new ForbidResult();
            }
        }
    }
} 