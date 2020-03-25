using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public interface IPersonalTrainingsRepository
    {
        Task InsertPersonalTraining(PersonalTrainingEntity personalTraining);
        Task<List<PersonalTrainingEntity>> GetAllPersonalTrainings();
        Task<PersonalTrainingEntity> GetPersonalTrainingByID(string id);
        Task DeleteTraining(string id);
        Task AddReportFromAthlete(string id, string report);
        Task AddResults(string id, List<SetEntity> result);
    }
}
