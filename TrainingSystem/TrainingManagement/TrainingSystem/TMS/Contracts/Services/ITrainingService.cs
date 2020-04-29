using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public interface ITrainingService
    {
        Task<bool> CheckIfTrainingBelongToRightPerson(string personId, string trainingId);
        Task<bool> CheckIfPersonalTrainingBelongToRightPerson(string personId, string trainingId);
        Task<bool> CheckIfPersonCanUpdatePersonalTrainingReport(string personId, string trainingId);
    }
}
