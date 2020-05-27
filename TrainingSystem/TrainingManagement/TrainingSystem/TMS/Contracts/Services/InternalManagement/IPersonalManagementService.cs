using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS.Contracts.Services.InternalManagement
{
    public interface IPersonalManagementService
    {
        Task AddCompetitionToListOrSetNewRecord(string athleteId, CompetitionEntity competition);
        Task<string> SendInviteToAnother(string senderId,string senderRole, string receiverId);
        Task AcceptInvitation(string senderId, string role, string receiverId);
      
    }
}
