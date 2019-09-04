using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcClient
{
    public static class HttpContextExtensions
    {
        public static string UserId(this HttpContext ctx)
        {
            return ctx.User?.Claims?.Where(q => q.Type == "sub").Select(q => q.Value).FirstOrDefault();
        }

        public static string UserName(this HttpContext ctx)
        {
            return ctx.User?.Claims.Where(q => q.Type == "name").Select(q => q.Value).FirstOrDefault();
        }
    }
}
