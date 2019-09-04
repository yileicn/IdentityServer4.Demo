using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MvcClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                //我们设置为DefaultChallengeSchemeto，"oidc"因为当我们需要用户登录时，我们将使用OpenID Connect协议。
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            //AddOpenIdConnect用于配置执行OpenID Connect协议的处理程序。这Authority表明我们信任IdentityServer。
            //然后，我们通过ClientId。识别此客户。 SaveTokens用于在cookie中保留来自IdentityServer的令牌（稍后将需要它们）。
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "http://localhost:5000";
                options.RequireHttpsMetadata = false;

                options.ClientId = "mvc";
                //Hybrid
                options.ClientSecret = "secret";
                options.ResponseType = "code id_token";

                options.SaveTokens = true;
                //Hybrid  设置从UserInfoEndpoint获取claims信息
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ClaimActions.MapCustomJson("testRole", p=> p["role"]?.ToString());

                options.Scope.Add("api1.read");
                options.Scope.Add("api1.write");
                //Hybrid
                options.Scope.Add("offline_access");
                //事件监听
                options.Events.OnTokenResponseReceived = (context) =>
                {
                    var accessToken = context.TokenEndpointResponse.AccessToken;
                    context.Response.Cookies.Append("act", accessToken);
                    return Task.CompletedTask;
                };


            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
