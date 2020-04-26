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
    public class PersonalTrainingRepository:IPersonalTrainingsRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task InsertPersonalTraining(PersonalTrainingEntity personalTraining)
        {
            var PersonalTrainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);
            await PersonalTrainingRepo.InsertOneAsync(personalTraining);
        }

        public async Task InsertManyPersonalTrainings(List<PersonalTrainingEntity> personalTrainings)
        {
            var PersonalTrainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);
            await PersonalTrainingRepo.InsertManyAsync(personalTrainings);
        }

        public async Task<List<PersonalTrainingEntity>> GetAllPersonalTrainings()
        {
            var trainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);
            var response = await trainingRepo.FindAsync(x => true, new DatabaseFindOptions());

            return response.Items;
        }
        public async Task<List<PersonalTrainingEntity>> GetAllPersonalTrainingsCoach(string coach)
        {
            var trainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);
            var filterBuilder = Builders<PersonalTrainingEntity>.Filter;
            var filter = filterBuilder.Eq("coach", ObjectId.Parse(coach));
            var response = await trainingRepo.FindAsync(filter);


            return response.Items;
        }

        public async Task<List<PersonalTrainingEntity>> GetAssignedTrainingsByDate(string date, string coach)
        {
            var firstdateInDateTime = DateTime.Parse(date).AddHours(-6);
            var LastdateInDateTime = DateTime.Parse(date).AddHours(6);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var totalFirstTime = (long)(DateTime.SpecifyKind(firstdateInDateTime, DateTimeKind.Local).ToUniversalTime() - epoch).TotalMilliseconds;
            var totalLastTime = (long)(DateTime.SpecifyKind(LastdateInDateTime, DateTimeKind.Local).ToUniversalTime() - epoch).TotalMilliseconds;

            var filterBuilder = Builders<PersonalTrainingEntity>.Filter;
            var filter = filterBuilder.Gt("day", totalFirstTime)
                & filterBuilder.Lt("day", totalLastTime)
                & filterBuilder.Eq("coach", ObjectId.Parse(coach));

            var trainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);
            var response = await trainingRepo.FindAsync(filter, new DatabaseFindOptions(){});

            return response.Items;
        }
        public async Task<List<PersonalTrainingEntity>> GetAllPersonalTrainingsAthlete(string athlete)
        {
            var trainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);
            var filterBuilder = Builders<PersonalTrainingEntity>.Filter;
            var filter = filterBuilder.Eq("athlete", ObjectId.Parse(athlete));
            var response = await trainingRepo.FindAsync(filter);

            return response.Items;
        }



        public async Task<PersonalTrainingEntity> GetPersonalTrainingByID(string id)
        {
            var trainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);
            var response = await trainingRepo.FindOneByIdAsync(id);

            return response;
        }
        public async Task<PersonalTrainingEntity> GetPersonalTrainingByDate(string date, string athlete)
        {
            var firstdateInDateTime = DateTime.Parse(date).AddHours(-6);
            var LastdateInDateTime = DateTime.Parse(date).AddHours(6);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var totalFirstTime = (long)(DateTime.SpecifyKind(firstdateInDateTime, DateTimeKind.Local).ToUniversalTime() - epoch).TotalMilliseconds;
            var totalLastTime = (long)(DateTime.SpecifyKind(LastdateInDateTime, DateTimeKind.Local).ToUniversalTime() - epoch).TotalMilliseconds;

            var filterBuilder = Builders<PersonalTrainingEntity>.Filter;
            var filter = filterBuilder.Gt("day", totalFirstTime)
                & filterBuilder.Lt("day", totalLastTime)
                & filterBuilder.Eq("athlete", ObjectId.Parse(athlete));

            var trainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);
            var response = await trainingRepo.FindOneAsync(filter);

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
        public async Task AddResultsAndReport(string id, Results result)
        {
            var trainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);           

            var filter = Builders<PersonalTrainingEntity>.Update.AddToSetEach(x => x.Results, result.results)
                .Set(x => x.AthleteReport, result.report);
            await trainingRepo.UpdateOneAsync(x => x.Id == id, filter, new DatabaseUpdateOneOptions() { BypassDocumentValidation = true });
        }

        public async Task ClearResults(string id)
        {
            var trainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);

            var filter = Builders<PersonalTrainingEntity>.Update.Unset(x => x.Results);
                
            await trainingRepo.UpdateOneAsync(x => x.Id == id, filter, new DatabaseUpdateOneOptions() { BypassDocumentValidation = true });
        }

        public async Task<bool> CheckIfCoachHasTrainingInChoosenDay(DateTime day, string coachId)
        {
            var dayStart = day.AddHours(-6);
            var dayEnd = day.AddHours(6);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var totalFirstTime = (long)(DateTime.SpecifyKind(dayStart, DateTimeKind.Local).ToUniversalTime() - epoch).TotalMilliseconds;
            var totalSecondTime = (long)(DateTime.SpecifyKind(dayEnd, DateTimeKind.Local).ToUniversalTime() - epoch).TotalMilliseconds;
            var trainingRepo = new CodeMashRepository<PersonalTrainingEntity>(Client);

            var filterBuilder = Builders<PersonalTrainingEntity>.Filter;

            var filter = filterBuilder.Gte(x => x.Day, dayStart) &
                filterBuilder.Lte(x=>x.Day, dayEnd) 
                & filterBuilder.Eq("coach", ObjectId.Parse(coachId));

            var response = await trainingRepo.FindAsync(filter, new DatabaseFindOptions());

            if (response.Items.Count !=0)
            {
                return true;
            }

            return false;
        }

    }
}
