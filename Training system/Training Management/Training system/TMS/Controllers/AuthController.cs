using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
                AuthRepository = new AuthRepository(),
                AthleteRepository = new AthleteRepository(),
                CoachRepository = new CoachRepository(),
                ConsumerRepository = new ConsumerRepository()
            };
        }

        [HttpGet("aaa")]
        public async Task<IActionResult> Logaaain(User user)
        {




            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {

            var jwt = await authService.Login(user);
            if (jwt == null)
            {
                return BadRequest("this user doesn't exist");
            }

            return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
           var response = await authService.Register(user);
            if (response == null)
            {
                return Ok();
            }
            else
            {
                return BadRequest(response);
            }
        }


    }
}
