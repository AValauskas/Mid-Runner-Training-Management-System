using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMS.Contracts.Repositories;
using TMS.Contracts.Repositories.InternalManagement;
using TMS.Contracts.Services.InternalManagement;

namespace TMS.Services.InternalManagement
{
    public class PersonalManagementService:IPersonalManagementService
    {
        public IConsumerRepository ConsumerRepository { get; set; }      
        public IAggregateRepository AggregateRepository { get; set; }

        public async Task AddCompetitionToListOrSetNewRecord(string athleteId, CompetitionEntity competition)
        {
            competition.Time = double.Parse(String.Format("{0:0.00}", competition.Time));
            await ConsumerRepository.AddNewCompetition(athleteId, competition);

            var consumer = await ConsumerRepository.CheckIfRecordExist(athleteId, competition);

            if (consumer!= null)
            {
                consumer = await ConsumerRepository.CheckIfBiggerPersonalTimeExist(athleteId, competition);
                if (consumer == null)
                {
                    await ConsumerRepository.UpdatePersonalRecord(athleteId, competition);
                }
            }
            else
            {
                await ConsumerRepository.AddNewPersonalBest(athleteId, competition);
            }                     
        }




        public async Task<string> SendInviteToAnother(string senderId, string senderRole, string receiverId)
        {
            if (receiverId== senderId)
            {
                return "you can't invite yourself";
            }
            if (receiverId.Length!=24)
            {
                return "wrong code was written";
            }
            if (senderRole=="Athlete")
            {
                var receiver = await ConsumerRepository.FindConsumerById(receiverId);
                if (receiver==null)
                {
                    return "this person not exist";
                }

                if (receiver.InviteFrom.Contains(senderId))
                {
                    return "You already sent invitation";
                }              
                if (receiver.Athletes.Contains(senderId))
                {
                    return "You already belong to this trainer";
                }
                if (receiver.Friends.Contains(senderId))
                {
                    return "You are already friends with this athlete";
                }
                if (receiver.Role == "Admin")
                {
                    return "You can't invite admin";
                }
                var self = await ConsumerRepository.FindConsumerById(senderId);
                if (self.InviteFrom.Contains(receiverId))
                {
                    return "You already have request from this user";
                }
               
                if (receiver.Role=="Coach")
                {
                    var athlete = await AggregateRepository.AthletePersonalCoachAggregate(senderId);
                    if (athlete.Count != 0)
                    {
                        return "You already have coach";
                    }
                }             

            }
            if (senderRole == "Coach")
            {
                var coach = await ConsumerRepository.FindConsumerById(senderId);

                if (coach.Athletes.Contains(receiverId))
                {
                    return "You already train this athlete";
                }
                if (coach.InviteFrom.Contains(receiverId))
                {
                    return "Athlete already sent you invitation";
                }
                var athlete = await ConsumerRepository.FindConsumerById(receiverId);
                if (athlete == null)
                {
                    return "this person not exist";
                }
                if (athlete.InviteFrom.Contains(senderId))
                {
                    return "You have already sent invitation";
                }
                if (athlete.Role == "Coach")
                {
                    return "You can invite only athletes";
                }
                if (athlete.Role == "Admin")
                {
                    return "You can't invite admin";
                }
                var athlete2 = await AggregateRepository.AthletePersonalCoachAggregate(receiverId);
                if (athlete2.Count != 0)
                {
                    return "This athlete already have coach";
                }
            }       

            await ConsumerRepository.SendInviteToAnother(senderId, receiverId);
            return null;
        }
        public async Task AcceptInvitation(string senderId,string role, string receiverId)
        {
            
            if (role == "Athlete")
            {
                var consumer = await ConsumerRepository.FindConsumerById(senderId);

                if (consumer.Role == "Coach")
                {
                    await ConsumerRepository.AcceptInvitationCoach(senderId, receiverId);
                }
                else {
                    await ConsumerRepository.AcceptInvitationAthlete(senderId, receiverId);
                }              
            }
            else if (role == "Coach")
            {
                await ConsumerRepository.AcceptInvitationCoach(receiverId, senderId);
               
            }
            await ConsumerRepository.DeleteInvitation(receiverId, senderId);
        }

     
      
    }
}
