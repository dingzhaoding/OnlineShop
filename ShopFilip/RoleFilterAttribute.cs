using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip
{
    public class RoleFilterAttribute : ActionFilterAttribute
    {
        public string Role { get; set; }
        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            // Assume that we have user identity because Authorize is also
            // applied
            var user = ctx.HttpContext.User;
            if (!user.IsInRole(Role))
            {
                ctx.Result = new RedirectResult("/account/ErrorPage");
            }
        }
    }
}
