using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FM.SSO
{
    /// <summary>
    /// Profile就是用户资料，ids 4里面定义了一个IProfileService的接口用来获取用户的一些信息，
    /// 主要是为当前的认证上下文绑定claims。我们可以实现IProfileService从外部创建claim扩展到ids4里面。然后返回
    /// </summary>
    public class ProfileService : IProfileService
    {
        public ProfileService()
        {

        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //depending on the scope accessing the user data.
                //var claims = context.Subject.Claims.ToList();

                //set issued claims to return
                //context.IssuedClaims = claims.ToList();

                //获取UserId
                var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
                var user = Config.GetUsers().FirstOrDefault(p => p.SubjectId == subjectId);

                //过滤出请求的ClaimTypes
                var claims = user.Claims.Where(p => context.RequestedClaimTypes.Contains(p.Type)).ToList();

                //添加RoleClaim
                var roleClaim = user.Claims.FirstOrDefault(p => p.Type == "role");
                if (roleClaim != null)
                {
                    claims.Add(roleClaim);
                }                

                context.IssuedClaims.AddRange(claims);
            }
            catch (Exception ex)
            {
                //log your error
                Console.WriteLine(ex);
            } 
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
        }
    }
}
