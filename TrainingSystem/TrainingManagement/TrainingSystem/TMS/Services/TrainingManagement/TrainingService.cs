using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMS.Contracts.Repositories.TrainingManagement;
using TMS.Contracts.Services.TrainingManagement;

namespace TMS.Services.TrainingManagement
{
    public class TrainingService: ITrainingService
    {
        public ITrainingsRepository TrainingRepo { get; set; }
 
        public async Task<bool> CheckIfTrainingBelongToRightPerson(string personId, string trainingId)
        {
            var train = await TrainingRepo.GetTrainingByID(trainingId);

            if (train.Owner!= personId)
                return false;

            return true;
        }       

      
        public async Task<List<TrainingEntity>> InsertTraining(TrainingEntity training, string id)
        {
            await TrainingRepo.InsertTraining(training);

            var train = await TrainingRepo.GetAllAvailableTrainings(id);
            train.Select(x =>
            {
                var startN = x.TrainingType.IndexOf("name");
                var endN = x.TrainingType.IndexOf("taxonomy");
                x.TrainingTypeName = x.TrainingType.Substring(startN + 8, endN - startN - 15);
                startN = x.TrainingType.IndexOf("id");
                endN = x.TrainingType.IndexOf("name");
                x.TrainingType = x.TrainingType.Substring(startN + 6, endN - startN - 13);
                return x;
            }).ToList();

            return train;
        }

        public async Task<List<TrainingEntity>> UpdateTraining(TrainingEntity training, string id)
        {
            await TrainingRepo.ReplaceTraining(training);

            var train = await TrainingRepo.GetAllAvailableTrainings(id);
            train.Select(x =>
            {
                var startN = x.TrainingType.IndexOf("name");
                var endN = x.TrainingType.IndexOf("taxonomy");
                x.TrainingTypeName = x.TrainingType.Substring(startN + 8, endN - startN - 15);
                startN = x.TrainingType.IndexOf("id");
                endN = x.TrainingType.IndexOf("name");
                x.TrainingType = x.TrainingType.Substring(startN + 6, endN - startN - 13);
                return x;
            }).ToList();

            return train;
        }

        public async Task<List<TrainingEntity>> DeleteTraining(string training, string id)
        {
            await TrainingRepo.DeleteTraining(training);

            var train = await TrainingRepo.GetAllAvailableTrainings(id);
            train.Select(x =>
            {
                var startN = x.TrainingType.IndexOf("name");
                var endN = x.TrainingType.IndexOf("taxonomy");
                x.TrainingTypeName = x.TrainingType.Substring(startN + 8, endN - startN - 15);
                startN = x.TrainingType.IndexOf("id");
                endN = x.TrainingType.IndexOf("name");
                x.TrainingType = x.TrainingType.Substring(startN + 6, endN - startN - 13);
                return x;
            }).ToList();

            return train;
        }
    }
}

