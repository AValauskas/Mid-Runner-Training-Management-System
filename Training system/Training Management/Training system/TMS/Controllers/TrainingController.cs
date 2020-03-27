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
    [Authorize(Roles = "Coach, Athlete", AuthenticationSchemes = "coach, athlete")]
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingsReposiry trainingRepo;
        public TrainingController()
        {
            trainingRepo = new TrainingsRepository();
        }
        [Authorize(Roles = "Coach")]
        [HttpPost]
        public async Task<IActionResult> PostTraining([FromBody] TrainingEntity training)
        {
            await trainingRepo.InsertTraining(training);

            return Ok(training);
        }

        [HttpGet]
        public async Task<IActionResult> GetTrainings()
        {
            var training = await trainingRepo.GetAllTrainings();

            return Ok(training);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTraining([FromRoute] string id)
        {
            var training = await trainingRepo.GetTrainingByID(id);

            return Ok(training);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraining([FromRoute] string id)
        {
            await trainingRepo.DeleteTraining(id);

            return Ok(id);
        }


        [HttpPut()]
        public async Task<IActionResult> UpdateTraining([FromBody] TrainingEntity training)
        {
            await trainingRepo.ReplaceTraining(training);

            return Ok(training);
        }



    }
}