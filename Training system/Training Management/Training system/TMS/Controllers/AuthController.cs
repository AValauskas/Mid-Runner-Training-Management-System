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
        private readonly IConsumerRepository consumerRepository;
        public AuthController()
        {
            authRepo = new AuthRepository();
            consumerRepository = new ConsumerRepository();
            authService = new AuthService()
            {
                AuthRepository = new AuthRepository(),
                ConsumerRepository = new ConsumerRepository(),
                EmailRepository = new EmailRepository()
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
        [HttpGet("confirm/{id}")]
        public async Task<IActionResult> Register([FromRoute] string Id)
        {
             await authRepo.VerifyRegister(Id);
           
             return Ok();           
            
        }

        [HttpPost("resetpassword/{id}")]
        public async Task<IActionResult> SendReminderToEmail([FromBody] string email)
        {
            var response = await authService.RequestForNewPassword(email);
            if (response != null)
            {
                return BadRequest(response);
            }

            return Ok();
        }

        [HttpGet("confirmreset/{id}")]
        public async Task<IActionResult> ConfirmResetPassword([FromRoute] string Id)
        {
            var pass = RandomWord.RandomString(10);
            await authService.ResetPassword(Id, pass);

            return Ok();
        }
    }
}
