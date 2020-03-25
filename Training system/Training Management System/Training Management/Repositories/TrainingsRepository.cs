using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Training_Management
{
    public class TrainingsRepository:ITrainingsReposiry
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task InsertTraining(TrainingEntity training)
        {
            var athleteRepository = new CodeMashRepository<TrainingEntity>(Client);
            await athleteRepository.InsertOneAsync(training);
        }

        public async Task<List<TrainingEntity>> GetAllTrainings()
        {
            var trainingRepo = new CodeMashRepository<TrainingEntity>(Client);
            var response = await trainingRepo.FindAsync(x => true, new DatabaseFindOptions());

            return response.Items;
        }
        public async Task<TrainingEntity> GetTrainingByID(string id)
        {
            var trainingRepo = new CodeMashRepository<TrainingEntity>(Client);
            var response = await trainingRepo.FindOneByIdAsync(id);

            return response;
        }

        public async Task ReplaceTraining(TrainingEntity training)
        {
            var trainingRepo = new CodeMashRepository<TrainingEntity>(Client);
            await trainingRepo.ReplaceOneAsync(x=> x.Id == training.Id, training);
        }

        public async Task DeleteTraining(string id)
        {
            var trainingRepo = new CodeMashRepository<TrainingEntity>(Client);
            await trainingRepo.DeleteOneAsync(ObjectId.Parse(id));
        }
    }
}
