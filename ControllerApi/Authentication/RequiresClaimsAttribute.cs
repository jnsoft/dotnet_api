using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ControllerApi.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequiresClaimsAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _claim_name;
        private readonly string _claim_value;

        public RequiresClaimsAttribute(string claim_name, string claim_value)
        {
            _claim_name = claim_name;
            _claim_value = claim_value;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(!context.HttpContext.User.HasClaim(_claim_name, _claim_value)) 
            {
                context.Result = new ForbidResult();
            }
        }
    }
}

