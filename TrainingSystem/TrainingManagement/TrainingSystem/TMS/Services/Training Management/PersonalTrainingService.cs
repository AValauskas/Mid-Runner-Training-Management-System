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
        public async Task<List<TrainingsWhichAreAssignedByDate>> DeletePersonalTrain(string personId, string trainingId)
        {
            var train = await PersonalTrainingRepo.GetPersonalTrainingByID(trainingId);

            if (train.CoachId != personId)
            {
                return null;
            }
            await PersonalTrainingRepo.DeleteTraining(trainingId);
            var trainings = await AggregateRepository.TrainingsWhichAssignedByDate(train.Day.ToString(), train.CoachId);

            return trainings;
        }


        public async Task<bool> CheckIfCanUpdate(string personId, string trainingId)
        {
            var train = await PersonalTrainingRepo.GetPersonalTrainingByID(trainingId);


            if (train.CoachId == personId || train.AthleteId == personId)
            {
                return true;
            }

            return false;

        }

        public async Task<List<PersonInfo>> GetAthletesIfFree(string idCoach, string date)
        {
            var trainDate = DateTime.Parse(date);
            var exist = await PersonalTrainingRepo.CheckIfCoachHasTrainingInChoosenDay(trainDate, idCoach);
            if (exist)
            {
                return new List<PersonInfo>();
            }
            else
            {
                return await AggregateRepository.GetAllCoachAthletesAggregate(idCoach);
            }

        }


    }
}
