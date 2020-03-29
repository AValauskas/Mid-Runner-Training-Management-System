using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class PersonalTrainingService:IPersonalTrainingService
    {
        public PersonalTrainingRepository PersonalTrainingRepo { get; set; }
        //public async Task<bool> CheckIfAthleteStillNotAddedChoosenDay(DateTime day, string AthleteId)
        //{
        //    var train = await PersonalTrainingRepo.GetTrainingByID(trainingId);

        //    if (train.Owner != personId)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
    }
}
