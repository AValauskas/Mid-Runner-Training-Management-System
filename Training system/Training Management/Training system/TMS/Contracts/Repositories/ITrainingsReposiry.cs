using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public interface ITrainingsReposiry
    {
        Task InsertTraining(TrainingEntity training);
        Task<List<TrainingEntity>> GetAllTrainings();
        Task<List<TrainingEntity>> GetPersonalTrainings(string owner);
        Task<List<TrainingEntity>> GetAllAvailableTrainings();
        Task<TrainingEntity> GetTrainingByID(string id);
        Task ReplaceTraining(TrainingEntity training);
        Task DeleteTraining(string id);

    }
}
