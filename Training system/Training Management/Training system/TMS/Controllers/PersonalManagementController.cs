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
        private readonly IConsumerRepository ConsumerRepository;
        public PersonalManagementController()
        {

            ConsumerRepository = new ConsumerRepository();
            personalManagementService = new PersonalManagementService()
            {
                ConsumerRepository = new ConsumerRepository()
            };
        }

        //--------------------------------Competition/personal results-------------------------


        [Authorize(Roles = "Athlete,Coach")]
        [HttpGet("record/outside")]
        public async Task<IActionResult> GetRecordsOutside()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idAthlete = cla[1].Value;
            var consumer = await ConsumerRepository.FindConsumerById(idAthlete);

            return Ok(consumer.Records);


        }
        [Authorize(Roles = "Athlete,Coach")]
        [HttpGet("record/inside")]
        public async Task<IActionResult> GetRecordsInside()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idAthlete = cla[1].Value;
            var consumer = await ConsumerRepository.FindConsumerById(idAthlete);
            var record = consumer.Records.Where(x => x.Place == InsideOutside.Inside.ToString());
            return Ok(record);
        }

        [Authorize(Roles = "Athlete,Coach")]
        [HttpGet("competitions/inside")]
        public async Task<IActionResult> GetCompetitionsInside()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idAthlete = cla[1].Value;
            var consumer = await ConsumerRepository.FindConsumerById(idAthlete);
            var record = consumer.Records.Where(x => x.Place == InsideOutside.Outside.ToString());
            return Ok(consumer.Records);
        }
        [Authorize(Roles = "Athlete,Coach")]
        [HttpGet("competitions/inside")]
        public async Task<IActionResult> GetCompetitionsOutside()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idAthlete = cla[1].Value;
            var consumer = await ConsumerRepository.FindConsumerById(idAthlete);
            var competitions = consumer.Competitions.Where(x => x.Place == InsideOutside.Outside.ToString());
            List<CompetitionEntity> SortedList = competitions.OrderBy(o => o.Time).ToList();
            return Ok(consumer.Records);
        }

        [Authorize(Roles = "Athlete, Coach")]
        [HttpPatch]
        public async Task<IActionResult> AddCompetitionTime ([FromBody] CompetitionEntity competition)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idAthlete = cla[1].Value;

            await personalManagementService.AddCompetitionToListOrSetNewRecord(idAthlete, competition);
            return Ok();
        }




        //----------------------------Invites---------------------
        [Authorize(Roles = "Athlete, Coach")]
        [HttpPatch("invite")]
        public async Task<IActionResult> SendInvite([FromBody] ConsumerEntity consumer)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var SenderId = cla[1].Value;
            var SenderRole = cla[0].Value;

            var result = await personalManagementService.SendInviteToAnother(SenderId, SenderRole, consumer.Id);
            if (result == null)
            {
                return Ok(result);
            }
            {
                return BadRequest(result);
            }
            
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
            var invites = await personalManagementService.GetInvitations( receiverId);
            return Ok(invites);
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
            var invites = await personalManagementService.GetInvitations(receiverId);
            return Ok(invites);
        }

        [Authorize(Roles = "Athlete, Coach")]
        [HttpGet("invitations")]
        public async Task<IActionResult> GetInvitations()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idAthlete = cla[1].Value;
            var invites = await personalManagementService.GetInvitations(idAthlete);

            return Ok(invites);
        }

    }
}