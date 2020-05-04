using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class TrainingService: ITrainingService
    {
        public ITrainingsReposiry TrainingRepo { get; set; }
        public IPersonalTrainingsRepository PersonalTrainingsRepository { get; set; }
        public async Task<bool> CheckIfTrainingBelongToRightPerson(string personId, string trainingId)
        {
            var train = await TrainingRepo.GetTrainingByID(trainingId);

            if (train.Owner!= personId)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CheckIfPersonalTrainingBelongToRightPerson(string personId, string trainingId)
        {
            var train = await PersonalTrainingsRepository.GetPersonalTrainingByID(trainingId);

            if (train.CoachId != personId)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CheckIfPersonCanUpdatePersonalTrainingReport(string personId, string trainingId)
        {
            var train = await PersonalTrainingsRepository.GetPersonalTrainingByID(trainingId);

                                 
            if (train.CoachId == personId)
            {
                return true;
            }
            if (train.AthleteId == personId)
            {
                return true;
            }
            return false;
        }
        public async Task<List<TrainingEntity>> InsertTraining(TrainingEntity training, string id)
        {
            await TrainingRepo.InsertTraining(training);

            var train = await TrainingRepo.GetAllTrainingsIncludedPersonal(id);
            train.Select(x => {
                var StartN = x.TrainingType.IndexOf("name");
                var EndN = x.TrainingType.IndexOf("taxonomy");
                x.TrainingTypeName = x.TrainingType.Substring(StartN + 8, EndN - StartN - 15);
                StartN = x.TrainingType.IndexOf("id");
                EndN = x.TrainingType.IndexOf("name");
                x.TrainingType = x.TrainingType.Substring(StartN + 6, EndN - StartN - 13);
                return x;
            }).ToList();

            return train;
        }

        public async Task<List<TrainingEntity>> UpdateTraining(TrainingEntity training, string id)
        {
            await TrainingRepo.ReplaceTraining(training);

            var train = await TrainingRepo.GetAllTrainingsIncludedPersonal(id);
            train.Select(x => {
                var StartN = x.TrainingType.IndexOf("name");
                var EndN = x.TrainingType.IndexOf("taxonomy");
                x.TrainingTypeName = x.TrainingType.Substring(StartN + 8, EndN - StartN - 15);
                StartN = x.TrainingType.IndexOf("id");
                EndN = x.TrainingType.IndexOf("name");
                x.TrainingType = x.TrainingType.Substring(StartN + 6, EndN - StartN - 13);
                return x;
            }).ToList();

            return train;
        }

        public async Task<List<TrainingEntity>> DeleteTraining(string training, string id)
        {
            await TrainingRepo.DeleteTraining(training);

            var train = await TrainingRepo.GetAllTrainingsIncludedPersonal(id);
            train.Select(x => {
                var StartN = x.TrainingType.IndexOf("name");
                var EndN = x.TrainingType.IndexOf("taxonomy");
                x.TrainingTypeName = x.TrainingType.Substring(StartN + 8, EndN - StartN - 15);
                StartN = x.TrainingType.IndexOf("id");
                EndN = x.TrainingType.IndexOf("name");
                x.TrainingType = x.TrainingType.Substring(StartN + 6, EndN - StartN - 13);
                return x;
            }).ToList();

            return train;
        }
    }
}

