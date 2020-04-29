using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class PersonalTrainingService:IPersonalTrainingService
    {
        public IPersonalTrainingsRepository PersonalTrainingRepo { get; set; }
        public IAggregateRepository AggregateRepository { get; set; }
        public async Task ProcessPersonalTraining (PersonalTraining training)
        {
            var personalTraining = new List<PersonalTrainingEntity>();
            foreach (var athlete in training.AthleteIds)
            {
                personalTraining.Add(new PersonalTrainingEntity()
                {
                    AthleteId= athlete,
                    CoachId= training.CoachId,
                    Day = training.Day,
                    Description= training.Description,
                    Place = training.Place,
                    TrainTemplateId = training.TrainTemplateId
                });
            }
             await PersonalTrainingRepo.InsertManyPersonalTrainings(personalTraining);          
        }

       
    }
}
