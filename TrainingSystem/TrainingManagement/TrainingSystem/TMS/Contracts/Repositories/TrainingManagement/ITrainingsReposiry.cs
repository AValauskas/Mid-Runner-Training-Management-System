using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS.Contracts.Repositories.TrainingManagement
{
    public interface ITrainingsRepository
    {
        Task InsertTraining(TrainingEntity training);
        Task<List<TrainingEntity>> GetAllTrainings();
        Task<List<TrainingEntity>> GetPersonalTrainings(string owner);
        Task<List<TrainingEntity>> GetAllAvailableTrainings(string owner);
        Task<TrainingEntity> GetTrainingByID(string id);
        Task ReplaceTraining(TrainingEntity training);
        Task DeleteTraining(string id);
        Task<List<TrainingEntity>> GetTrainingsByType(string typeId, string owner);
    }
}
