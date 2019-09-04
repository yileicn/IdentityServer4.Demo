using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer
{
    public static class HttpContextExtensions
    {
        public static bool HasScope(this HttpContext ctx,string scopeName)
        {
            return ctx.User?.HasClaim(p => p.Type == "scope" && p.Value == scopeName) ?? false;
        }

        public static string ClientId(this HttpContext ctx)
        {
            return ctx.User?.Claims?.Where(q => q.Type == "client_id").Select(q => q.Value).FirstOrDefault();
        }
    }
}
