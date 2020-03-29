using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class PersonalManagementService:IPersonalManagementService
    {
        public IConsumerRepository ConsumerRepository { get; set; }
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

        public async Task<string> SendInviteToAnother(string senderId, string receiverId)
        {
            var consumer = await ConsumerRepository.FindConsumerById(receiverId);
            if (consumer.Role == "Admin")
            {
                return "You can't invite admin, only another coach";
            }
            await ConsumerRepository.SendInviteToAnother(senderId, receiverId);
            return "Invite have been succesfully sent";
        }
        public async Task AcceptInvitation(string senderId,string role, string receiverId)
        {
            if (role == "Athlete")
            {
                await ConsumerRepository.AceptInvitation(senderId, receiverId);

                await ConsumerRepository.DeleteInvitation(receiverId, senderId);
            }
            else if (role == "Coach")
            {
                await ConsumerRepository.AceptInvitation(receiverId, senderId);
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

    }
}
