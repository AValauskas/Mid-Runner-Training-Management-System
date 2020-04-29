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

    }
}
