using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcClient.Models;
using Newtonsoft.Json.Linq;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Secure()
        {
            ViewData["Message"] = "Secure page.";

            return View();
        }

        /// <summary>
        /// 调用读取Api
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CallReadApi()
        {
            //获取Access
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            //调用Api
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("http://localhost:5001/api/values");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return new JsonResult(new { StatusCode = 0, Result = JArray.Parse(content) });
            }
            else
            {
                return new JsonResult(new { response.StatusCode, ErrorMessage = response.ReasonPhrase});
            }

        }

        /// <summary>
        /// 调用写入Api
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CallWriteApi()
        {
            //获取Access
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            //调用Api
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.DeleteAsync("http://localhost:5001/api/values/1");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return new JsonResult(new { StatusCode = 0,Result = content});
            }
            else
            {
                return new JsonResult(new { response.StatusCode, Message = response.ReasonPhrase });
            }
        }
    }
}
