using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blood_Donation_Website.Filters
{
    public class HospitalAdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.IsInRole("Bệnh viện") && !context.HttpContext.User.IsInRole("Quản trị viên"))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            }
            base.OnActionExecuting(context);
        }
    }
} 