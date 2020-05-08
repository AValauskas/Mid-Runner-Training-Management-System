﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public interface IPersonalTrainingService
    {
        Task ProcessPersonalTraining(PersonalTraining training);
        Task<List<TrainingsWhichAreAssignedByDate>> DeletePersonalTrain(string personId, string trainingId);
        Task<bool> CheckIfPersonCanUpdatePersonalTrainingReport(string personId, string trainingId);
        Task<List<PersonInfo>> GetAthletesIfFree(string idCoach, string date);
    }
}
