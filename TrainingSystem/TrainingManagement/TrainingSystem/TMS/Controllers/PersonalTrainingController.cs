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
        public IPersonalTrainingsRepository trainingRepo;
        public IAggregateRepository aggregateRepo;
        public IPersonalTrainingService personalTrainingService;
        public PersonalTrainingController()
        {
            aggregateRepo = new AggregateRepository();
            trainingRepo = new PersonalTrainingRepository();

            personalTrainingService = new PersonalTrainingService()
            {
                AggregateRepository = aggregateRepo,
                PersonalTrainingRepo = trainingRepo
            };
        }
        //NAUDOJAMA
        [Authorize(Roles = "Coach")]
        [HttpPost]
        public async Task<IActionResult> CreatePersonalTraining([FromBody] PersonalTraining training)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idCoach = cla[1].Value;
            training.CoachId = idCoach;
            training.Day = training.Day.AddHours(2);

            await personalTrainingService.ProcessPersonalTraining(training);

            return Ok();
        }
        //naudojama norint gauti asmenines treniruotes atletui ir jas atvaizduoti
        [Authorize(Roles = "Athlete")]
        [HttpGet("athlete")]
        public async Task<IActionResult> GetPersonalTrainingsByAthlete()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idAthlete = cla[1].Value;
            

            var training = await trainingRepo.GetAllPersonalTrainingsAthlete(idAthlete);
            return Ok(training);
        }

        [Authorize(Roles = "Coach")]
        [HttpGet("countByBusy")]
        //gaunama dienos ir atvaizduojama kiek atletų iš kiek atitinkamą dieną jau yra užimti
        public async Task<IActionResult> GetPersonalTrainingsCount()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idCoach = cla[1].Value;
            var busyTrains = await aggregateRepo.GetCoachAssignedTrainingsCount(idCoach);
            return Ok(busyTrains);
        }

        [Authorize(Roles = "Coach")]
        [HttpGet("coach/{date}")]
        public async Task<IActionResult> GetAssignedPersonalTrainings([FromRoute] string date)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idCoach = cla[1].Value;
            var training = await aggregateRepo.TrainingsWhichAssignedByDate(date, idCoach);
            return Ok(training);
        }


        [Authorize(Roles = "Coach")]
        [HttpGet("coach")]

        public async Task<IActionResult> GetPersonalTrainingsWhichGiven()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idCoach= cla[1].Value;
            var training = await trainingRepo.GetAllPersonalTrainingsCoach(idCoach);
           
            return Ok(training);
        }

  
        


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonalTraining([FromRoute] string id)
        {
            var training = await trainingRepo.GetPersonalTrainingByID(id);
            return Ok(training);
        }


        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetPersonalTrainingByDate([FromRoute] string date)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idAthlete = cla[1].Value;

            var training = await trainingRepo.GetPersonalTrainingByDate(date, idAthlete);
            
            return Ok(training);
        }

        [Authorize(Roles = "Coach, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraining([FromRoute] string id)
        {

            var claims = User.Claims;
            var cla = claims.ToList();
            var idConsumer = cla[1].Value;

            var trainings = await personalTrainingService.DeletePersonalTrain(idConsumer, id);
            if (trainings == null)
            {
                return Unauthorized("This isn't yours training");
            }

           

            return Ok(trainings);
        }
        ///------------------------------------ištrinti blaiviam-----------------
        [Authorize(Roles = "Athlete")]
        [HttpPatch("Report/{id}")]
        public async Task<IActionResult> UpdateTrainingReport([FromRoute] string id, [FromBody] Results report )
        {

            var claims = User.Claims;
            var cla = claims.ToList();
            var idConsumer = cla[1].Value;

            if (!await personalTrainingService.CheckIfPersonCanUpdatePersonalTrainingReport(idConsumer, id))
            {
                return Unauthorized("This isn't yours training");
            }

            await trainingRepo.AddReportFromAthlete(id, report.report);

            return Ok();
        }

        ///-------------------------permesti į servisą blaiviam---------------------------
        [Authorize(Roles = "Coach, Athlete")]
        [HttpPatch("Results/{id}")]
        public async Task<IActionResult> UpdateTrainingResults([FromRoute] string id, [FromBody] Results report)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idConsumer = cla[1].Value;

            if (!await personalTrainingService.CheckIfPersonCanUpdatePersonalTrainingReport(idConsumer, id))
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

        ////----------PersonalTraining--------------
        [Authorize(Roles = "Coach")]
        [HttpGet("athleteList/{date}")]
        public async Task<IActionResult> GetFreeAthletes([FromRoute] string date) ////athletes who still doesn't have any training
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idCoach = cla[1].Value;
            var athletes = await aggregateRepo.GetFreeAthletesByDayAggregate(date, idCoach);
            if (athletes.Count == 0)
            {
                athletes = await personalTrainingService.GetAthletesIfFree(idCoach, date);

                return Ok(athletes);

            }
            return Ok(athletes);

        }
    }
}