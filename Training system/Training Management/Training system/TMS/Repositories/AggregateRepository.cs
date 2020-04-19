using CodeMash.Client;
using CodeMash.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class AggregateRepository :IAggregateRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);
        public async Task<List<CoachAssignedTrains>> GetCoachAssignedTrainingsCount(string coachId)
        {
            var service = new CodeMashRepository<PersonalTrainingEntity>(Client);
            var trains = await service.AggregateAsync<CoachAssignedTrains>(Guid.Parse("5638f150-6b59-47d9-af9c-0eebeb6d1c33"), new AggregateOptions
            {
                Tokens = new Dictionary<string, string>()
                {
                       { "id", coachId },
                }
            });

            return trains;
        }
        public async Task<List<PersonalRecords>> RecordAggregate(string consumerId)
        {
            var service = new CodeMashRepository<ConsumerEntity>(Client);
            var records = await service.AggregateAsync<PersonalRecords>(Guid.Parse("521f0ce6-bbfe-457c-9a91-5c82bde2eea3"), new AggregateOptions
            {
                Tokens = new Dictionary<string, string>()
                {
                       { "id", consumerId },
                }
            });

            return records;
        }

        public async Task<List<CompetitionsAggregate>> CompetitionsAggregate(string consumerId)
        {
            var service = new CodeMashRepository<ConsumerEntity>(Client);
            var competitions = await service.AggregateAsync<CompetitionsAggregate>(Guid.Parse("df55fb69-8ebe-4365-bd22-024e96763c3c"), new AggregateOptions
            {
                Tokens = new Dictionary<string, string>()
                {
                       { "id", consumerId },
                }
            });

            return competitions;
        }

        public async Task<List<TrainingsWhichAreAssignedByDate>> TrainingsWhichAssignedByDate(string date, string coachId)
        {
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var dateFromDate = DateTime.Parse(date).AddHours(-5);
            var dateToDate = DateTime.Parse(date).AddHours(5);
            var dateFromFloat = dateFromDate.Subtract(startTime).TotalMilliseconds;
            var dateToFloat = dateToDate.Subtract(startTime).TotalMilliseconds;

            var service = new CodeMashRepository<PersonalTrainingEntity>(Client);
            var trains = await service.AggregateAsync<TrainingsWhichAreAssignedByDate>(Guid.Parse("f19360c6-20c9-444b-8c5f-3073b7bf573b"), new AggregateOptions
            {
                Tokens = new Dictionary<string, string>()
                {
                       { "id", coachId },
                       { "dateFrom", dateFromFloat.ToString() },
                       { "dateTo", dateToFloat.ToString() },
                }
            });

            return trains;
        }
    }
}
