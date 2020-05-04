using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class PersonalManagementService:IPersonalManagementService
    {
        public IConsumerRepository ConsumerRepository { get; set; }       
        public IPersonalTrainingsRepository PersonalTrainingsRepository { get; set; }
        public IAggregateRepository AggregateRepository { get; set; }

        public async Task AddCompetitionToListOrSetNewRecord(string AthleteId, CompetitionEntity competition)
        {
            competition.Time = double.Parse(String.Format("{0:0.00}", competition.Time));
            await ConsumerRepository.AddNewCompetition(AthleteId, competition);

            var consumer = await ConsumerRepository.CheckIfRecordExist(AthleteId, competition);

            if (consumer!= null)
            {
                consumer = await ConsumerRepository.CheckIfBiggerPersonalTimeExist(AthleteId, competition);
                if (consumer != null)
                {

                }
                else
                {
                    await ConsumerRepository.UpdatePersonalRecord(AthleteId, competition);
                }
            }
            else
            {
                await ConsumerRepository.AddNewPersonalBest(AthleteId, competition);
            }                     
        }




        public async Task<string> SendInviteToAnother(string senderId, string senderRole, string receiverId)
        {
            if (senderRole=="Athlete")
            {
                var receiver = await ConsumerRepository.FindConsumerById(receiverId);

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
                    var athlete = await AggregateRepository.FindOutIfAthleteHasCoachAggregate(senderId);
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
                var athlete2 = await AggregateRepository.FindOutIfAthleteHasCoachAggregate(receiverId);
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
                    await ConsumerRepository.AceptInvitationCoach(senderId, receiverId);
                }
                else {
                    await ConsumerRepository.AceptInvitationAthlete(senderId, receiverId);
                }              

                await ConsumerRepository.DeleteInvitation(receiverId, senderId);
            }
            else if (role == "Coach")
            {
                await ConsumerRepository.AceptInvitationCoach(receiverId, senderId);
                await ConsumerRepository.DeleteInvitation(receiverId, senderId);
            }         
        }

        public async Task DeclineInvitation(string senderId, string role, string receiverId)
        {
            if (role == "Athlete")
            {
                await ConsumerRepository.DeleteInvitation(receiverId, senderId);
            }
            else if (role == "Coach")
            {
                await ConsumerRepository.DeleteInvitation(receiverId, senderId);
            }
        }

        public async Task<List<PersonInfoForCoach>> GetAthletesIfFree(string idCoach, string date)
        {
             var trainDate = DateTime.Parse(date);                   
            var exist = await PersonalTrainingsRepository.CheckIfCoachHasTrainingInChoosenDay(trainDate, idCoach);
            if (exist)
            {
                return new List<PersonInfoForCoach>();
            }
            else
            {

                return await AggregateRepository.GetAllCoachAthletesAggregate(idCoach);
            }

        }

      
    }
}
