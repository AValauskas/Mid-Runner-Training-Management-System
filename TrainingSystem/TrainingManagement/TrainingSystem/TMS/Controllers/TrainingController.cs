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
        public ITrainingsReposiry trainingRepo;
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


          
            training.Select(x => {
                var StartN = x.TrainingType.IndexOf("name");
                var EndN = x.TrainingType.IndexOf("taxonomy");
                x.TrainingTypeName = x.TrainingType.Substring(StartN + 8, EndN - StartN - 15);
                StartN = x.TrainingType.IndexOf("id");
                EndN = x.TrainingType.IndexOf("name");
                x.TrainingType = x.TrainingType.Substring(StartN + 6, EndN - StartN - 13);
                return x;                
            }).ToList();
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
                var Start = x.TrainingType.IndexOf("name");
                var End = x.TrainingType.IndexOf("taxonomy");
                x.TrainingType = x.TrainingType.Substring(Start + 8, End - Start - 15);
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