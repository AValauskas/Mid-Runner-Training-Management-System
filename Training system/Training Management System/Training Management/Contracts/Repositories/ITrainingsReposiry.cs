using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Training_Management
{
    public interface ITrainingsReposiry
    {
        Task InsertTraining(TrainingEntity training);
        Task<List<TrainingEntity>> GetAllTrainings();
        Task<TrainingEntity> GetTrainingByID(string id);
        Task ReplaceTraining(TrainingEntity training);
        Task DeleteTraining(string id);

    }
}
