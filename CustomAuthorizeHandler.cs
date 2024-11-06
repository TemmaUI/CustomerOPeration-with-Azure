using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using System.Security.Claims;

namespace AzureCustomerOPeration.Attributes
{
    public class CustomAuthorizeHandler : AuthorizationHandler<CustomAuthorizeAttribute>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizeAttribute requirement)
        {
            var roleClaim = context.User.FindFirstValue(ClaimTypes.Role);

            if (roleClaim != null && roleClaim.Equals(requirement.Role, StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            await Task.CompletedTask;
        }
    }
}
