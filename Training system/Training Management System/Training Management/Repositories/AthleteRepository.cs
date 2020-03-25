using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Training_Management
{   
    public class AthleteRepository:IAthleteRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public AthleteEntity FindAthlete(User user)
        {
            /*  var athleteRepository = new CodeMashRepository<AthleteEntity>(Client);
              var filterBuilder = Builders<AthleteEntity>.Filter;
              var filter = filterBuilder.Eq(x => x., user.email) &
              filterBuilder.Eq(x => x.Password, user.Password);

             var athlete = athleteRepository.FindOne(filter);

             return athlete;*/
            return null;
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
