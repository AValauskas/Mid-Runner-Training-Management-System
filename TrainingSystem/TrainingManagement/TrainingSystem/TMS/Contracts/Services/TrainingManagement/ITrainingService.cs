using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS.Contracts.Services.TrainingManagement
{
    public interface ITrainingService
    {
        Task<bool> CheckIfTrainingBelongToRightPerson(string personId, string trainingId);
       
        Task<List<TrainingEntity>> InsertTraining(TrainingEntity training, string id);
        Task<List<TrainingEntity>> UpdateTraining(TrainingEntity training, string id);
        Task<List<TrainingEntity>> DeleteTraining(string training, string id);
    }
}
