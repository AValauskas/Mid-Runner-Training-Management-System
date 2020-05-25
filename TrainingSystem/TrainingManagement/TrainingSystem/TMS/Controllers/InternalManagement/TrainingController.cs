using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Contracts.Repositories.TrainingManagement;
using TMS.Contracts.Services.TrainingManagement;
using TMS.Repositories.TrainingManagement;
using TMS.Services.TrainingManagement;

namespace TMS.Controllers.InternalManagement
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Coach, Athlete, Admin", AuthenticationSchemes = "coach, athlete, admin")]
    public class TrainingController : ControllerBase
    {
        public ITrainingsRepository trainingRepo;
        public ITrainingService trainingService;
        public TrainingController()
        {
            trainingRepo = new TrainingsRepository();
            trainingService = new TrainingService()
            {
                TrainingRepo = trainingRepo
            };
        }
        /// <summary>
        /// Insert training
        /// </summary>
        /// <param name="training"></param>
        /// <returns></returns>
        [Authorize(Roles = "Coach, Admin")]
        [HttpPost]
       
        public async Task<IActionResult> PostTraining([FromBody] TrainingEntity training)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idTraining = cla[1].Value;
            training.Owner = idTraining;
            // await trainingRepo.InsertTraining(training);

            var train = await trainingService.InsertTraining(training, idTraining);
                       

            return Ok(train);
           
        }
        /// <summary>
        /// Get trainings for athlete and coach first step
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAvailableTrainings()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idConsumer = cla[1].Value;
            var training = await trainingRepo.GetAllAvailableTrainings(idConsumer);
            
            training.Select(x =>
            {
                var startN = x.TrainingType.IndexOf("name");
                var endN = x.TrainingType.IndexOf("taxonomy");
                x.TrainingTypeName = x.TrainingType.Substring(startN + 8, endN - startN - 15);
                startN = x.TrainingType.IndexOf("id");
                endN = x.TrainingType.IndexOf("name");
                x.TrainingType = x.TrainingType.Substring(startN + 6, endN - startN - 13);
                return x;
            });
            return Ok(training);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("availableTrainings")]
        public async Task<IActionResult> GetAllTrainings()
        {
            var training = await trainingRepo.GetAllTrainings();

            return Ok(training);
        }
     /// <summary>
     /// training given by type
     /// </summary>
     /// <param name="id"></param>
     /// <returns></returns>
        
        [HttpGet("TrainingsByType/{id}")]
        public async Task<IActionResult> GetTrainingsByTaxonomy([FromRoute] string id)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idCoach = cla[1].Value;

            var training = await trainingRepo.GetTrainingsByType(id, idCoach);

            training.Select(x => {
                var start = x.TrainingType.IndexOf("name");
                var end = x.TrainingType.IndexOf("taxonomy");
                x.TrainingType = x.TrainingType.Substring(start + 8, end - start - 15);
                return x;
            }).ToList();
            return Ok(training);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTraining([FromRoute] string id)
        {
            var training = await trainingRepo.GetTrainingByID(id);
          
            return Ok(training);
        }
        /// <summary>
        /// Delete training
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

            var train = await trainingService.DeleteTraining(id, idPerson);

            return Ok(train);
        }
        /// <summary>
        /// update training
        /// </summary>
        /// <param name="training"></param>
        /// <returns></returns>
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
            var train = await trainingService.UpdateTraining(training, idPerson);

            return Ok(train);
        }



    }
}