using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS.Contracts.Services.TrainingManagement
{
    public interface IPersonalTrainingService
    {
        Task ProcessPersonalTraining(PersonalTraining training);
        Task<List<TrainingsWhichAreAssignedByDate>> DeletePersonalTrain(string personId, string trainingId);
        Task<bool> CheckIfCanUpdate(string personId, string trainingId);
        Task<List<PersonInfo>> GetAthletesIfFree(string idCoach, string date);
    }
}
