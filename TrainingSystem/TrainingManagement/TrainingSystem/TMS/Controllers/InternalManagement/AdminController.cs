using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Contracts.Repositories.InternalManagement;
using TMS.Contracts.Services.InternalManagement;
using TMS.Repositories.InternalManagement;
using TMS.Services.InternalManagement;

namespace TMS.Controllers.InternalManagement
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = " Admin", AuthenticationSchemes = "admin")]
    public class AdminController : ControllerBase
    {
        public IConsumerRepository ConsumerRepository;
        public IAuthService authService;
        public AdminController()
        {
            ConsumerRepository = new ConsumerRepository();
            authService = new AuthService()
            {
                AuthRepository = new AuthRepository(),
                ConsumerRepository = ConsumerRepository,
                EmailRepository = new EmailRepository()
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await ConsumerRepository.GetAllUsers();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("")]
        public async Task<IActionResult> InsertUser([FromBody] ConsumerEntity user)
        {
            if (user.Password.Length < 6)
            {
                return BadRequest("Password must contain at least 6 symbols");
            }
            var response = await authService.InsertNewAdmin(user);
            if (response == null)
            {
                var users = await ConsumerRepository.GetAllUsers();
                return Ok(users);
            }
            else
            {
                return BadRequest(response);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            await ConsumerRepository.DeleteUser(id);
            var users = await ConsumerRepository.GetAllUsers();
            return Ok(users);
        }
    }
}