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
                var coach = await ConsumerRepository.FindConsumerById(receiverId);
                if (coach.InviteFrom.Contains(senderId))
                {
                    return "You already sent invitation";
                }
                if (coach.Athletes.Contains(senderId))
                {
                    return "You already belong to his trainer";
                }
                if (coach.Role =="Athlete")
                {
                    return "You can invite only coaches";
                }
                var athlete = await ConsumerRepository.FindConsumerById(senderId);
                if (athlete.InviteFrom.Contains(receiverId))
                {
                    return "You already have request from this coach";
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
                    return "Athlete already sent invitation";
                }
                if (athlete.Role == "Coach")
                {
                    return "You can invite only athletes";
                }
            }
            var consumer = await ConsumerRepository.FindConsumerById(receiverId);
            if (consumer.Role == "Admin")
            {
                return "You can't invite admin, only another coach is available";
            }
            await ConsumerRepository.SendInviteToAnother(senderId, receiverId);
            return null;
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

        public async Task<List<InviteForm>> GetInvitations(string idMainUser)
        {
            List<InviteForm> invites = new List<InviteForm>();
            var consumer = await ConsumerRepository.FindConsumerById(idMainUser);
            foreach (var inviterId in consumer.InviteFrom)
            {
                var invaiter = await ConsumerRepository.FindConsumerById(inviterId);
                invites.Add(new InviteForm()
                {
                    Name = invaiter.Name,
                    Surname = invaiter.Surname,
                    IDInvitationFrom = inviterId
                });
            }

            return invites;
        }

        public async Task<List<PersonInfo>> GetAthletes(string idCoach, string date)
        {
            var trainDate = DateTime.Parse(date);
            var trainDateStart = trainDate.AddHours(-4);
            List<PersonInfo> athletes = new List<PersonInfo>();
            var consumer = await ConsumerRepository.FindConsumerById(idCoach);            
            foreach (var athleteId in consumer.Athletes)
            {             
                var isAdded = await PersonalTrainingsRepository.CheckIfAthleteisAddedInChoosenDay(trainDate, athleteId);
                if (!isAdded)
                {
                    var athlete = await ConsumerRepository.FindConsumerById(athleteId);
                    athletes.Add(new PersonInfo()
                    {
                        Name = athlete.Name,
                        Surname = athlete.Surname,
                        IdPerson = athleteId
                    });

                }              
            }
            return athletes;
        }

      
    }
}
