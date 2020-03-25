using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class CoachRepository:ICoachRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task<CoachEntity> FindCoach(User user)
        {
            /*  var athleteRepository = new CodeMashRepository<CoachEntity>(Client);
              var filterBuilder = Builders<CoachEntity>.Filter;
              var filter = filterBuilder.Eq(x => x.Username, user.Username) &
              filterBuilder.Eq(x => x.Password, user.Password);

              var coach = await athleteRepository.FindOneAsync(filter);

              return coach;*/
            return null;
        }
    }
}
