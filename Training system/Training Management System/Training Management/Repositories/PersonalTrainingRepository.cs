using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Training_Management
{
    public class PersonalTrainingRepository:IPersonalTrainingsRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task InsertPersonalTraining(PersonalTrainingEntity personalTraining)
        {
            var PersonalTrainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);
            await PersonalTrainingRepo.InsertOneAsync(personalTraining);
        }

        public async Task<List<PersonalTrainingEntity>> GetAllPersonalTrainings()
        {
            var trainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);
            var response = await trainingRepo.FindAsync(x => true, new DatabaseFindOptions());

            return response.Items;
        }
        public async Task<PersonalTrainingEntity> GetPersonalTrainingByID(string id)
        {
            var trainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);
            var response = await trainingRepo.FindOneByIdAsync(id);

            return response;
        }


        public async Task DeleteTraining(string id)
        {
            var trainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);
            await trainingRepo.DeleteOneAsync(ObjectId.Parse(id));
        }

        public async Task AddReportFromAthlete(string id, string report)
        {
            var trainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);

            var filter = Builders<PersonalTrainingEntity>.Update.Set(x=>x.AthleteReport, report);
            await trainingRepo.UpdateOneAsync(x => x.Id == id, filter, null);
        }

        public async Task AddResults(string id, List<SetEntity> result)
        {
            var trainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);

            var filter = Builders<PersonalTrainingEntity>.Update.AddToSetEach(x => x.Results, result);
            await trainingRepo.UpdateOneAsync(x => x.Id == id, filter, new DatabaseUpdateOneOptions() { BypassDocumentValidation = true }) ;
        }

    }
}
