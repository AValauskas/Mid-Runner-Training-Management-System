using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TMS
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        public AuthController()
        {
            authService = new AuthService()
            {
                AthleteRepository = new AthleteRepository(),
                CoachRepository = new CoachRepository()
            };
        }

        [HttpGet("aaa")]
        public async Task<IActionResult> Logaaain(User user)
        {




            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(User user)
        {

            authService.Login(new User() { Email = "Aur", Password = "Val" }); ;


            return null;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(User user)
        {


            return null;
        }
    }
}
