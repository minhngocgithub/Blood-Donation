using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blood_Donation_Website.Filters
{
    public class DoctorOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.IsInRole("Doctor"))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            }
            base.OnActionExecuting(context);
        }
    }
} 