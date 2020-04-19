using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace TMS
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepo;
        private readonly IAuthService authService;
        public AuthController()
        {
            authRepo = new AuthRepository();
            authService = new AuthService()
            {
                AuthRepository = new AuthRepository(),
                ConsumerRepository = new ConsumerRepository()
            };
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {

            var jwt = await authService.Login(user);
            if (jwt == null)
            {
                return BadRequest("this user doesn't exist");
            }
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            var jwtToken = new JwtToken() { Token = token };
            var json = JsonConvert.SerializeObject(jwtToken);
            return Ok(json); 
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
        [HttpPost("confirm/{id}")]
        public async Task<IActionResult> Register([FromRoute] string Id)
        {
             await authRepo.VerifyRegister(Id);
           
             return Ok();
            
            
        }

    }
}
