using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace TMS
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class AuthController : ControllerBase
    {
        public IAuthRepository authRepo;
        public IAuthService authService;
        private readonly IEmailRepository emailRepo;
        private readonly IConsumerRepository consumerRepository;
        public AuthController()
        {
            emailRepo = new EmailRepository();
            authRepo = new AuthRepository();
            consumerRepository = new ConsumerRepository();
            authService = new AuthService()
            {
                AuthRepository = authRepo,
                ConsumerRepository = consumerRepository,
                EmailRepository = emailRepo,
            };
        }

      

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] ConsumerEntity user)
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
        public async Task<IActionResult> Register([FromBody] ConsumerEntity user)
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
        [HttpPatch("confirm/{id}")]
        public async Task<IActionResult> CompleteRegister([FromRoute] string Id)
        {
            if (Id.Length != 24)
            {
                return BadRequest("Wrong id given");
            }           

            await authRepo.VerifyRegister(Id);
           
             return Ok();           
            
        }

        [HttpPost("resetpassword/{email}")]
        public async Task<IActionResult> RequestForNewPassword([FromRoute] string email)
        {
            var response = await authService.RequestForNewPassword(email);
            if (response != null)
            {
                return BadRequest(response);
            }

            return Ok();
        }

        [HttpPatch("confirmreset/{id}")]
        public async Task<IActionResult> ConfirmPasswordReset([FromRoute] string Id)
        {
            if (Id.Length != 24)
            {
               return BadRequest("Wrong id given");
            }
            var pass = RandomWord.RandomString(10);
            await authService.ResetPassword(Id, pass);

            return Ok();
        }


    }
}
