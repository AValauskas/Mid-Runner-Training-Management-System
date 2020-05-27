using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMS.Contracts.Repositories.InternalManagement;

namespace TMS.Repositories.InternalManagement
{
    public class ConsumerRepository : IConsumerRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);
        public async Task<List<ConsumerEntity>> GetAllUsers()
        {
            var consumerRepository = new CodeMashRepository<ConsumerEntity>(Client);     

            var consumer = await consumerRepository.FindAsync();
            return consumer.Items;
        }


        public async Task<ConsumerEntity> FindConsumerByEmail(string email)
        {
            var consumerRepository = new CodeMashRepository<ConsumerEntity>(Client);
            var filterBuilder = Builders<ConsumerEntity>.Filter;
            var filter = filterBuilder.Eq(x => x.Email, email);

            var consumer = await consumerRepository.FindOneAsync(filter);

            return consumer;
        }
        public async Task<ConsumerEntity> FindConsumerById(string id)
        {
            var consumerRepository = new CodeMashRepository<ConsumerEntity>(Client);
            var consumer = await consumerRepository.FindOneByIdAsync(id);

            return consumer;
        }

        public async Task AcceptInvitationCoach(string coachId, string AthleteId)
        {
            var consumerRepo = new CodeMashRepository<ConsumerEntity>(Client);

            var filter = Builders<ConsumerEntity>.Update.AddToSet(x => x.Athletes, AthleteId);
            await consumerRepo.UpdateOneAsync(x => x.Id == coachId, filter, new DatabaseUpdateOneOptions() { BypassDocumentValidation = true });

        }

        public async Task AcceptInvitationAthlete(string sender, string receiver)
        {
            var consumerRepo = new CodeMashRepository<ConsumerEntity>(Client);

            var filter = Builders<ConsumerEntity>.Update.AddToSet(x => x.Friends, sender);
            await consumerRepo.UpdateOneAsync(x => x.Id == receiver, filter, new DatabaseUpdateOneOptions() { BypassDocumentValidation = true });
            var filter2 = Builders<ConsumerEntity>.Update.AddToSet(x => x.Friends, receiver);
            await consumerRepo.UpdateOneAsync(x => x.Id == sender, filter, new DatabaseUpdateOneOptions() { BypassDocumentValidation = true });

        }
        public async Task DeleteInvitation(string receiverId, string senderId)
        {
            var consumerRepo = new CodeMashRepository<ConsumerEntity>(Client);

            var filter = Builders<ConsumerEntity>.Update.Pull("invitefrom", ObjectId.Parse(senderId));
            await consumerRepo.UpdateOneAsync(x => x.Id == receiverId, filter, new DatabaseUpdateOneOptions() { BypassDocumentValidation = true });
            
        }


        public async Task AddNewCompetition(string athleteId, CompetitionEntity competition)
        {
            var consumerRepository = new CodeMashRepository<ConsumerEntity>(Client);

            var filter = Builders<ConsumerEntity>.Update.Push(x => x.Competitions, competition);
            await consumerRepository.UpdateOneAsync(x => x.Id == athleteId, filter, new DatabaseUpdateOneOptions() { BypassDocumentValidation = true });                       
        }


        public async Task<ConsumerEntity> CheckIfRecordExist(string athleteId, CompetitionEntity competition)
        {
            var consumerRepository = new CodeMashRepository<ConsumerEntity>(Client);

            var filterBuilder = Builders<ConsumerEntity>.Filter;
            var subfilter = filterBuilder.Eq("distance", competition.Distance) & filterBuilder.Eq("place", competition.Place);
            var filter = filterBuilder.Eq(x => x.Id, athleteId) & filterBuilder.ElemMatch("records", subfilter);
           // var filter = filterBuilder.Eq(x => x.Id, athleteId) & filterBuilder.Eq("records.distance", competition.Distance) & filterBuilder.Eq("records.place", competition.Place);                

            var updateBuilder = Builders<ConsumerEntity>.Update;
            
            var result = await consumerRepository.FindOneAsync(filter);
            return result;
        }

        public async Task<ConsumerEntity> CheckIfBiggerPersonalTimeExist(string athleteId, CompetitionEntity competition)
        {
            var consumerRepository = new CodeMashRepository<ConsumerEntity>(Client);

            var filterBuilder = Builders<ConsumerEntity>.Filter;
            var subfilter = filterBuilder.Eq("distance", competition.Distance)
                & filterBuilder.Eq("place", competition.Place)
                & filterBuilder.Lte("time", competition.Time);

            var filter = filterBuilder.Eq(x => x.Id, athleteId) & filterBuilder.ElemMatch("records", subfilter);
            var result = await consumerRepository.FindOneAsync(filter);
            return result;
        }


        public async Task UpdatePersonalRecord(string athleteId, CompetitionEntity competition)
        {
            var consumerRepository = new CodeMashRepository<ConsumerEntity>(Client);

            var filterBuilder = Builders<ConsumerEntity>.Filter;
            var subfilter = filterBuilder.Eq("distance", competition.Distance) & filterBuilder.Eq("place", competition.Place) & filterBuilder.Gte("time", competition.Time);
            var filter = filterBuilder.Eq(x => x.Id, athleteId) &
                (filterBuilder.ElemMatch("records", subfilter));

            var updateBuilder = Builders<ConsumerEntity>.Update;
            var update = updateBuilder.Set(doc => doc.Records[-1], competition);

            var result = await consumerRepository.UpdateOneAsync(
                filter,
                update,
                new DatabaseUpdateOneOptions()
            );

        }

        public async Task AddNewPersonalBest(string athleteId, CompetitionEntity competition)
        {
            var consumerRepository = new CodeMashRepository<ConsumerEntity>(Client);
            
            var updateBuilder = Builders<ConsumerEntity>.Update;
            var update = updateBuilder.Push(doc => doc.Records, competition);

            var result = await consumerRepository.UpdateOneAsync(x => x.Id == athleteId, update);
        }


        public async Task SendInviteToAnother(string senderId, string receiverId)
        {
            var consumerRepository = new CodeMashRepository<ConsumerEntity>(Client);

            var updateBuilder = Builders<ConsumerEntity>.Update;
            var update = updateBuilder.Push(doc => doc.InviteFrom, senderId);

            var result = await consumerRepository.UpdateOneAsync(x => x.Id == receiverId, update);

        }

        public async Task DeleteUser(string userId)
        {
            var consumerRepository = new CodeMashRepository<ConsumerEntity>(Client);
            var result = await consumerRepository.DeleteOneAsync(x => x.Id == userId);
        }
    }
}
