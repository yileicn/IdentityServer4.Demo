using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Authorize(Roles = "admin")]//通过角色控制
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        // GET api/admin
        [HttpGet]
        public List<string> Get()
        {
            return new List<string> { "admin", "yilei" };
        }
    }
}