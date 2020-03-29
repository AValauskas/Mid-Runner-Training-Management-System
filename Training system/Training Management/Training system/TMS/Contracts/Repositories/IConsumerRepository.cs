﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public interface IConsumerRepository
    {
        Task<ConsumerEntity> FindConsumer(User user);
        Task AddNewCompetition(string athleteId, CompetitionEntity competition);
        Task UpdatePersonalRecord(string athleteId, CompetitionEntity competition);
        Task<ConsumerEntity> CheckIfRecordExist(string athleteId, CompetitionEntity competition);
        Task AddNewPersonalBest(string athleteId, CompetitionEntity competition);
        Task<ConsumerEntity> CheckIfBiggerPersonalTimeExist(string athleteId, CompetitionEntity competition);
        Task SendInviteToAnother(string senderId, string receiverId);
        Task<ConsumerEntity> FindConsumerById(string id);
        Task AceptInvitation(string coachId, string AthleteId);
        Task DeleteInvitation(string receiverId, string senderId);
    }
}
