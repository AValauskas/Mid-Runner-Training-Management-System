using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public interface IAggregateRepository
    {
        Task<List<CoachAssignedTrains>> GetCoachAssignedTrainingsCount(string coachId);
        Task<List<TrainingsWhichAreAssignedByDate>> TrainingsWhichAssignedByDate(string date, string coachId);
        Task<List<PersonalRecords>> RecordAggregate(string consumerId);
        Task<List<CompetitionsAggregate>> CompetitionsAggregate(string consumerId);
        Task<List<PersonInForCoach>> GetFreeAthletesByDayAggregate(string date, string coachId);
        Task<List<PersonInForCoach>> GetAllCoachAthletesAggregate(string coachId);
        Task<List<PersonInfo>> GetAllInvitersAggregate(string consumerId);
        Task<List<PersonInfo>> FindOutIfAthleteHasCoachAggregate(string consumerId);
        Task<List<PersonInfo>> GetUserFriendsAggregate(string consumerId);
    }
}
