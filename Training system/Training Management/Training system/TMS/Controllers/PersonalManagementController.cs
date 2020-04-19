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
        private readonly IAuthService authService;
        private readonly IAggregateRepository aggregateRepo;
        public PersonalManagementController()
        {
            aggregateRepo = new AggregateRepository();
               authService = new AuthService()
            {
                AuthRepository = new AuthRepository(),
                ConsumerRepository = new ConsumerRepository()
            };

            ConsumerRepository = new ConsumerRepository();
            personalManagementService = new PersonalManagementService()
            {
                ConsumerRepository = new ConsumerRepository(),
                PersonalTrainingsRepository= new PersonalTrainingRepository()               
            };
        }

        //--------------------------------Competition/personal results-------------------------

       
        

       /* [Authorize(Roles = "Athlete,Coach")]
        [HttpGet("record/outside")]
        public async Task<IActionResult> GetRecordsOutside()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idAthlete = cla[1].Value;
            var consumer = await ConsumerRepository.FindConsumerById(idAthlete);

            return Ok(consumer.Records);


        }*/
        [Authorize(Roles = "Athlete,Coach")]
        [HttpGet("records")]
        public async Task<IActionResult> GetRecords()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idConsumer = cla[1].Value;
            var records = await aggregateRepo.RecordAggregate(idConsumer);
            records.Select(x => { x.Records.Select(y => { y.DateString = y.Date.ToString("yyyy-MM-dd"); return y; }).ToList(); return x;}).ToList();
            return Ok(records);
        }

        [Authorize(Roles = "Athlete,Coach")]
        [HttpGet("competitions")]
        public async Task<IActionResult> GetCompetitions()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idConsumer = cla[1].Value;
            var competitions = await aggregateRepo.CompetitionsAggregate(idConsumer);
            competitions.Select(x => { x.Records.Select(y => { y.DateString = y.Date.ToString("yyyy-MM-dd"); return y; }).ToList(); return x; }).ToList();
            return Ok(competitions);
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

        ////----------PersonalTraining--------------
        [Authorize(Roles = "Coach")]
        [HttpGet("athleteList/{date}")]
        public async Task<IActionResult> GetAthletesWhoAreStillWithoutTrain([FromRoute] string date)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idAthlete = cla[1].Value;
            var athletes = await personalManagementService.GetAthletes(idAthlete, date);
            return Ok(athletes);
        }


        //-----------------------------PersonalInfo     
        [HttpGet("personal")]
        public async Task<IActionResult> GetPersonalInfo()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idConsumer = cla[1].Value;
            var consumer = await ConsumerRepository.FindConsumerById(idConsumer);

            return Ok(consumer);
        }


        [HttpPatch("password")]
        public async Task<IActionResult> ChangePassword(User user)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idConsumer = cla[1].Value;
            await authService.ChangePassword(idConsumer, user.Password);

            return Ok();

        }

    }
}