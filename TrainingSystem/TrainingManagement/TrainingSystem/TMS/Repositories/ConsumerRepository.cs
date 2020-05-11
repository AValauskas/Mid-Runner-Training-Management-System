using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class ConsumerRepository : IConsumerRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);
        public async Task<ConsumerEntity> FindConsumer(ConsumerEntity user)
        {
            var ConsumerRepository = new CodeMashRepository<ConsumerEntity>(Client);
            var filterBuilder = Builders<ConsumerEntity>.Filter;
            var filter = filterBuilder.Eq(x => x.Email, user.Email) &
            filterBuilder.Eq(x => x.Password, user.Password);

            var consumer = await ConsumerRepository.FindOneAsync(filter);

            return consumer;
        }
        public async Task<List<ConsumerEntity>> GetAllUsers()
        {
            var ConsumerRepository = new CodeMashRepository<ConsumerEntity>(Client);     

            var consumer = await ConsumerRepository.FindAsync();
            return consumer.Items;
        }


        public async Task<ConsumerEntity> FindConsumerByEmail(string email)
        {
            var ConsumerRepository = new CodeMashRepository<ConsumerEntity>(Client);
            var filterBuilder = Builders<ConsumerEntity>.Filter;
            var filter = filterBuilder.Eq(x => x.Email, email);

            var consumer = await ConsumerRepository.FindOneAsync(filter);

            return consumer;
        }
        public async Task<ConsumerEntity> FindConsumerById(string id)
        {
            var ConsumerRepository = new CodeMashRepository<ConsumerEntity>(Client);
            var consumer = await ConsumerRepository.FindOneByIdAsync(id);

            return consumer;
        }

        public async Task AceptInvitationCoach(string coachId, string AthleteId)
        {
            var consumerRepo = new CodeMashRepository<ConsumerEntity>(Client);

            var filter = Builders<ConsumerEntity>.Update.AddToSet(x => x.Athletes, AthleteId);
            await consumerRepo.UpdateOneAsync(x => x.Id == coachId, filter, new DatabaseUpdateOneOptions() { BypassDocumentValidation = true });

        }

        public async Task AceptInvitationAthlete(string sender, string receiver)
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
            var ConsumerRepository = new CodeMashRepository<ConsumerEntity>(Client);

            var filter = Builders<ConsumerEntity>.Update.Push(x => x.Competitions, competition);
            await ConsumerRepository.UpdateOneAsync(x => x.Id == athleteId, filter, new DatabaseUpdateOneOptions() { BypassDocumentValidation = true });                       
        }


        public async Task<ConsumerEntity> CheckIfRecordExist(string athleteId, CompetitionEntity competition)
        {
            var ConsumerRepository = new CodeMashRepository<ConsumerEntity>(Client);

            var filterBuilder = Builders<ConsumerEntity>.Filter;
            var subfilter = filterBuilder.Eq("distance", competition.Distance) & filterBuilder.Eq("place", competition.Place);
            var filter = filterBuilder.Eq(x => x.Id, athleteId) & filterBuilder.ElemMatch("records", subfilter);
           // var filter = filterBuilder.Eq(x => x.Id, athleteId) & filterBuilder.Eq("records.distance", competition.Distance) & filterBuilder.Eq("records.place", competition.Place);                

            var updateBuilder = Builders<ConsumerEntity>.Update;
            
            var result = await ConsumerRepository.FindOneAsync(filter);
            return result;
        }

        public async Task<ConsumerEntity> CheckIfBiggerPersonalTimeExist(string athleteId, CompetitionEntity competition)
        {
            var ConsumerRepository = new CodeMashRepository<ConsumerEntity>(Client);

            var filterBuilder = Builders<ConsumerEntity>.Filter;
            var subfilter = filterBuilder.Eq("distance", competition.Distance)
                & filterBuilder.Eq("place", competition.Place)
                 & filterBuilder.Lte("time", competition.Time);

            var filter = filterBuilder.Eq(x => x.Id, athleteId) & filterBuilder.ElemMatch("records", subfilter);
            /* var filter = filterBuilder.Eq(x => x.Id, athleteId) 
                  & filterBuilder.Eq("records.distance", competition.Distance)
                   & filterBuilder.Eq("records.place", competition.Place)
                  & filterBuilder.Lte("records.time", competition.Time);*/

            var updateBuilder = Builders<ConsumerEntity>.Update;

            var result = await ConsumerRepository.FindOneAsync(filter);
            return result;
        }


        public async Task UpdatePersonalRecord(string athleteId, CompetitionEntity competition)
        {
            var ConsumerRepository = new CodeMashRepository<ConsumerEntity>(Client);

            var filterBuilder = Builders<ConsumerEntity>.Filter;
            var subfilter = filterBuilder.Eq("distance", competition.Distance) & filterBuilder.Eq("place", competition.Place) & filterBuilder.Gte("time", competition.Time);
            var filter = filterBuilder.Eq(x => x.Id, athleteId) &
                (filterBuilder.ElemMatch("records", subfilter));

            var updateBuilder = Builders<ConsumerEntity>.Update;
            var update = updateBuilder.Set(doc => doc.Records[-1], competition);

            var result = await ConsumerRepository.UpdateOneAsync(
                filter,
                update,
                new DatabaseUpdateOneOptions()
            );

        }

        public async Task AddNewPersonalBest(string athleteId, CompetitionEntity competition)
        {
            var ConsumerRepository = new CodeMashRepository<ConsumerEntity>(Client);
            
            var updateBuilder = Builders<ConsumerEntity>.Update;
            var update = updateBuilder.Push(doc => doc.Records, competition);

            var result = await ConsumerRepository.UpdateOneAsync(x => x.Id == athleteId, update);
        }


        public async Task SendInviteToAnother(string senderId, string receiverId)
        {
            var ConsumerRepository = new CodeMashRepository<ConsumerEntity>(Client);

            var updateBuilder = Builders<ConsumerEntity>.Update;
            var update = updateBuilder.Push(doc => doc.InviteFrom, senderId);

            var result = await ConsumerRepository.UpdateOneAsync(x => x.Id == receiverId, update);

        }

        public async Task DeleteUser(string userId)
        {
            var ConsumerRepository = new CodeMashRepository<ConsumerEntity>(Client);
            var result = await ConsumerRepository.DeleteOneAsync(x => x.Id == userId);
        }
    }
}
