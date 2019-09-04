using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MvcClient.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        public IActionResult GetContextUserInfo()
        {
            return new JsonResult(HttpContext.User);
        }

        public async Task<IActionResult> GetUserInfo()
        {
            //获取AccessToken
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return new JsonResult(disco.Error);
            }

            //获取用户信息
            var userInfo = await client.GetUserInfoAsync(new UserInfoRequest() { Token = accessToken, Address = disco.UserInfoEndpoint });
            return new JsonResult(userInfo.Json);
        }
    }
}