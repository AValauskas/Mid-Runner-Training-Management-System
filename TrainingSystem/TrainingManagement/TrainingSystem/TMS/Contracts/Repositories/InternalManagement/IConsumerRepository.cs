using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS.Contracts.Repositories.InternalManagement
{
    public interface IConsumerRepository
    {
        Task<ConsumerEntity> FindConsumerByEmail(string email);
        Task AddNewCompetition(string athleteId, CompetitionEntity competition);
        Task UpdatePersonalRecord(string athleteId, CompetitionEntity competition);
        Task<ConsumerEntity> CheckIfRecordExist(string athleteId, CompetitionEntity competition);
        Task AddNewPersonalBest(string athleteId, CompetitionEntity competition);
        Task<ConsumerEntity> CheckIfBiggerPersonalTimeExist(string athleteId, CompetitionEntity competition);
        Task SendInviteToAnother(string senderId, string receiverId);
        Task<ConsumerEntity> FindConsumerById(string id);
        Task AcceptInvitationCoach(string coachId, string athleteId);
        Task AcceptInvitationAthlete(string sender, string receiver);
        Task DeleteInvitation(string receiverId, string senderId);
        Task<List<ConsumerEntity>> GetAllUsers();
        Task DeleteUser(string userId);



    }
}
