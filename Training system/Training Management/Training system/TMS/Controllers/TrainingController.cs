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
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingsReposiry trainingRepo;
        private readonly ITrainingService trainingService;
        public TrainingController()
        {
            trainingRepo = new TrainingsRepository();
            trainingService = new TrainingService()
            {
                TrainingRepo = trainingRepo
            };
        }
        [Authorize(Roles = "Coach, Admin")]
        [HttpPost]
        public async Task<IActionResult> PostTraining([FromBody] TrainingEntity training)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idTraining = cla[1].Value;
            training.Owner = idTraining;
            await trainingRepo.InsertTraining(training);

            return Ok(training);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAvailableTrainings()
        {
            var training = await trainingRepo.GetAllAvailableTrainings();

            if (training == null)
            {
                return NotFound();
            }

            return Ok(training);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("availableTrainings")]
        public async Task<IActionResult> GetAllTrainings()
        {
            var training = await trainingRepo.GetAllTrainings();

            if (training == null)
            {
                return NotFound();
            }
            return Ok(training);
        }
        [Authorize(Roles = "Coach, Admin")]
        [HttpGet("personalTrainings")]
        public async Task<IActionResult> GetPersonalTrainings()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var claims = User.Claims;
            var cla = claims.ToList();
            var idTraining= cla[1].Value;
            var training = await trainingRepo.GetPersonalTrainings(idTraining);
            if (training == null)
            {
                return NotFound();
            }
            return Ok(training);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTraining([FromRoute] string id)
        {
            var training = await trainingRepo.GetTrainingByID(id);
            if (training == null)
            {
                return NotFound();
            }
            return Ok(training);
        }

        [Authorize(Roles = "Coach, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraining([FromRoute] string id)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idPerson = cla[1].Value;

            if (!await trainingService.CheckIfTrainingBelongToRightPerson(idPerson, id))
            {
                return Unauthorized("This isn't yours training");
            }
            

            await trainingRepo.DeleteTraining(id);

            return Ok("Succesfully deleted");
        }

        [Authorize(Roles = "Coach, Admin")]
        [HttpPut()]
        public async Task<IActionResult> UpdateTraining([FromBody] TrainingEntity training)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idPerson = cla[1].Value;
            
            if (!await trainingService.CheckIfTrainingBelongToRightPerson(idPerson, training.Id))
            {
                return Unauthorized("This isn't yours training");
            }
            training.Owner = idPerson;
            await trainingRepo.ReplaceTraining(training);


            return Ok(training);
        }



    }
}