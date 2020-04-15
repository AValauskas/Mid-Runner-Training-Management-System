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
    public class PersonalTrainingController : ControllerBase
    {
        private readonly IPersonalTrainingsRepository trainingRepo;
        private readonly ITrainingService trainingService;
        public PersonalTrainingController()
        {
            trainingRepo = new PersonalTrainingRepository();
            trainingService = new TrainingService()
            {
                PersonalTrainingsRepository = new PersonalTrainingRepository()
            };
        }

        [Authorize(Roles = "Coach")]
        [HttpPost]
        public async Task<IActionResult> CreatePersonalTraining([FromBody] PersonalTrainingEntity training)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idCoach = cla[1].Value;
            training.CoachId = idCoach;
            training.Day = training.Day.AddHours(2);

            if (await trainingRepo.CheckIfAthleteisAddedInChoosenDay(training.Day, training.AthleteId))
            {
                return BadRequest("Athlete in choosen day already have one training");
            }            

            await trainingRepo.InsertPersonalTraining(training);

            return Ok(training);
        }
        [Authorize(Roles = "Athlete")]
        [HttpGet("athlete")]
        public async Task<IActionResult> GetPersonalTrainingsByAthlete()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idAthlete = cla[1].Value;
            

            var training = await trainingRepo.GetAllPersonalTrainingsAthlete(idAthlete);
            if (training == null)
            {
                return NotFound();
            }
            return Ok(training);
        }



        [Authorize(Roles = "Coach")]
        [HttpGet("coach")]
        public async Task<IActionResult> GetPersonalTrainingsByCoach()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idCoach= cla[1].Value;


            var training = await trainingRepo.GetAllPersonalTrainingsCoach(idCoach);
            if (training == null)
            {
                return NotFound();
            }
            return Ok(training);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllPersonalTrainings()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idAdmin = cla[1].Value;


            var training = await trainingRepo.GetAllPersonalTrainings();
            if (training == null)
            {
                return NotFound();
            }
            return Ok(training);
        }
        


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonalTraining([FromRoute] string id)
        {
            var training = await trainingRepo.GetPersonalTrainingByID(id);
            if (training == null)
            {
                return NotFound();
            }
            return Ok(training);
        }


        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetPersonalTrainingByDate([FromRoute] string date)
        {
            var training = await trainingRepo.GetPersonalTrainingByDate(date);
            
            return Ok(training);
        }

        [Authorize(Roles = "Coach, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraining([FromRoute] string id)
        {

            var claims = User.Claims;
            var cla = claims.ToList();
            var idConsumer = cla[1].Value;

            if (!await trainingService.CheckIfPersonalTrainingBelongToRightPerson(idConsumer, id))
            {
                return Unauthorized("This isn't yours training");
            }

            await trainingRepo.DeleteTraining(id);

            return Ok();
        }
        [Authorize(Roles = "Athlete")]
        [HttpPatch("Report/{id}")]
        public async Task<IActionResult> UpdateTrainingReport([FromRoute] string id, [FromBody] Results report )
        {

            var claims = User.Claims;
            var cla = claims.ToList();
            var idConsumer = cla[1].Value;

            if (!await trainingService.CheckIfPersonCanUpdatePersonalTrainingReport(idConsumer, id))
            {
                return Unauthorized("This isn't yours training");
            }

            await trainingRepo.AddReportFromAthlete(id, report.report);

            return Ok();
        }
        [Authorize(Roles = "Coach, Athlete")]
        [HttpPatch("Results/{id}")]
        public async Task<IActionResult> UpdateTrainingResults([FromRoute] string id, [FromBody] Results report)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idConsumer = cla[1].Value;

            if (!await trainingService.CheckIfPersonCanUpdatePersonalTrainingReport(idConsumer, id))
            {
                return Unauthorized("This isn't yours training");
            }
            if (report.report==null && report.results != null)
            {
                await trainingRepo.ClearResults(id);
                await trainingRepo.AddResults(id, report.results);
            }
            else if (report.report != null && report.results != null)
            {
                await trainingRepo.ClearResults(id);
                await trainingRepo.AddResultsAndReport(id, report);
            }
            else if (report.report == null && report.results == null)
            {
                return BadRequest("results are ampty, fill all fields");
            }

            return Ok();
        }
    }
}