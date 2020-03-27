using CodeMash.Client;
using CodeMash.Repository;
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
        public async Task<ConsumerEntity> FindConsumer(User user)
        {
            var ConsumerRepository = new CodeMashRepository<ConsumerEntity>(Client);
            var filterBuilder = Builders<ConsumerEntity>.Filter;
            var filter = filterBuilder.Eq(x => x.Email, user.Email) &
            filterBuilder.Eq(x => x.Password, user.Password);

            var consumer = await ConsumerRepository.FindOneAsync(filter);

            return consumer;
        }
    }
}
