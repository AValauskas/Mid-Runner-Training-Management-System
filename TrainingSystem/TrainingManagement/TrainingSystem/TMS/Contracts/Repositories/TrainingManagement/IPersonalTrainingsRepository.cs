using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS.Contracts.Repositories.TrainingManagement
{
    public interface IPersonalTrainingsRepository
    {
        Task InsertPersonalTraining(PersonalTrainingEntity personalTraining);
        Task<List<PersonalTrainingEntity>> GetAllPersonalTrainings();
        Task<List<PersonalTrainingEntity>> GetAllPersonalTrainingsCoach(string coach);
        Task<List<PersonalTrainingEntity>> GetAllPersonalTrainingsAthlete(string athleteId);
        Task<List<PersonalTrainingEntity>> GetAssignedTrainingsByDate(string date, string coach);
        Task<PersonalTrainingEntity> GetPersonalTrainingByID(string id);
        Task DeleteTraining(string id);
        Task AddReportFromAthlete(string id, string report);
        Task AddResults(string id, List<SetEntity> result);
        Task AddResultsAndReport(string id, Results result);
        Task<bool> CheckIfCoachHasTrainingInChoosenDay(DateTime day, string coachId);
        Task<PersonalTrainingEntity> GetPersonalTrainingByDate(string date, string athlete);
        Task ClearResults(string id);
        Task InsertManyPersonalTrainings(List<PersonalTrainingEntity> personalTrainings);
    }
}
