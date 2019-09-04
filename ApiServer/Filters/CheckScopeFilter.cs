using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Filters
{
    public class CheckScopeFilterAttribute : Attribute, IActionFilter
    {
        private string scopeName { get; set; }

        public CheckScopeFilterAttribute(string scopeName)
        {
            this.scopeName = scopeName;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrWhiteSpace(scopeName))
            {
                if (!context.HttpContext.HasScope(scopeName))
                {
                    Console.WriteLine($"{context.HttpContext.ClientId()}未授权：{scopeName}");
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
            Console.WriteLine($"{context.HttpContext.ClientId()}已授权：{scopeName}");
        }
    }
}
