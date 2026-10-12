using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RaceDay.API.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SessionAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string[] _roles;

        public SessionAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                context.Result = new UnauthorizedObjectResult(
                    "Please log in first.");
                return;
            }

            if (_roles.Length > 0)
            {
                var role = context.HttpContext.Session
                    .GetString("UserRole");

                if (role == null || !_roles.Contains(
                    role, StringComparer.OrdinalIgnoreCase))
                {
                    context.Result = new ObjectResult(
                        "You do not have permission to access this resource.")
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                    return;
                }
            }

            await next();
        }
    }
}