using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TMS
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalTrainingController : ControllerBase
    {
        private readonly IPersonalTrainingsRepository trainingRepo;
        public PersonalTrainingController()
        {
            trainingRepo = new PersonalTrainingRepository();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonalTraining([FromBody] PersonalTrainingEntity training)
        {
            await trainingRepo.InsertPersonalTraining(training);

            return Ok(training);
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonalTrainings()
        {
            var training = await trainingRepo.GetAllPersonalTrainings();

            return Ok(training);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonalTraining([FromRoute] string id)
        {
            var training = await trainingRepo.GetPersonalTrainingByID(id);

            return Ok(training);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraining([FromRoute] string id)
        {
            await trainingRepo.DeleteTraining(id);

            return Ok();
        }

        [HttpPatch("Report/{id}")]
        public async Task<IActionResult> UpdateTrainingReport([FromRoute] string id, [FromBody] Results report )
        {
            await trainingRepo.AddReportFromAthlete(id, report.report);

            return Ok();
        }

        [HttpPatch("Results/{id}")]
        public async Task<IActionResult> UpdateTrainingResults([FromRoute] string id, [FromBody] Results report)
        {
            await trainingRepo.AddResults(id, report.results);

            return Ok();
        }
    }
}