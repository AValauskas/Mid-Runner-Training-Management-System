using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TMS
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Coach, Athlete, Admin", AuthenticationSchemes = "coach, athlete, admin")]
    public class PersonalManagementController : ControllerBase
    {

        private readonly IPersonalManagementService personalManagementService;

        public PersonalManagementController()
        {

            personalManagementService = new PersonalManagementService()
            {
                ConsumerRepository = new ConsumerRepository()
            };
        }

        [Authorize(Roles = "Athlete")]
        [HttpPatch]
        public async Task<IActionResult> AddCompetitionTime ([FromBody] CompetitionEntity competition)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idAthlete = cla[1].Value;

            await personalManagementService.AddCompetitionToListOrSetNewRecord(idAthlete, competition);
            return Ok();
        }

        [Authorize(Roles = "Athlete, Coach")]
        [HttpPatch("invite")]
        public async Task<IActionResult> SendInvite([FromBody] ConsumerEntity consumer)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var SenderId = cla[1].Value;

            var result = await personalManagementService.SendInviteToAnother(SenderId, consumer.Id);
            return Ok(result);
        }


        [Authorize(Roles = "Athlete, Coach")]
        [HttpPatch("AcceptInvite")]
        public async Task<IActionResult> AcceptInvitation([FromBody] ConsumerEntity consumer)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var receiverId = cla[1].Value;
            var receiverRole = cla[0].Value;

            await personalManagementService.AcceptInvitation(consumer.Id, receiverRole, receiverId);
            return Ok();
        }
        [Authorize(Roles = "Athlete, Coach")]
        [HttpPatch("DeclineInvite")]
        public async Task<IActionResult> DeclineInvitation([FromBody] ConsumerEntity consumer)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var receiverId = cla[1].Value;
            var receiverRole = cla[0].Value;

            await personalManagementService.DeclineInvitation(consumer.Id, receiverRole, receiverId);
            return Ok();
        }

    }
}