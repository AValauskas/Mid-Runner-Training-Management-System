using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{   
    public class AthleteRepository:IAthleteRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task<AthleteEntity> FindAthlete(User user)
        {
              var athleteRepository = new CodeMashRepository<AthleteEntity>(Client);
              var filterBuilder = Builders<AthleteEntity>.Filter;
              var filter = filterBuilder.Eq(x => x.Email, user.Email) &
              filterBuilder.Eq(x => x.Password, user.Password);

             var athlete = await athleteRepository.FindOneAsync(filter);

             return athlete;
        }

        public async Task<AthleteEntity> InsertAthlete(User user)
        {
            var athlete = new AthleteEntity()
            {
                //  Username = user.Username,

            };

           //var athlete = await athleteRepository.FindOneAsync(filter);

            return null;
        }


    }
}
