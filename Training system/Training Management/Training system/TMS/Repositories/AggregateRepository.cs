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
        public async Task<List<PersonInfo>> GetFreeAthletesByDayAggregate(string date, string coachId)
        {
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var dateFromDate = DateTime.Parse(date).AddHours(-5);
            var dateToDate = DateTime.Parse(date).AddHours(5);
            var dateFromFloat = dateFromDate.Subtract(startTime).TotalMilliseconds;
            var dateToFloat = dateToDate.Subtract(startTime).TotalMilliseconds;

            var service = new CodeMashRepository<PersonalTrainingEntity>(Client);
            var athletes = await service.AggregateAsync<PersonInfo>(Guid.Parse("1bc53f87-423e-44cb-8faa-d7ee4f5aba6d"), new AggregateOptions
            {
                Tokens = new Dictionary<string, string>()
                {
                       { "id", coachId },
                       { "dateFrom", dateFromFloat.ToString() },
                       { "dateTo", dateToFloat.ToString() },
                }
            });

            return athletes;

        }

        public async Task<List<PersonInfo>> GetAllCoachAthletesAggregate( string coachId)
        {
            var service = new CodeMashRepository<ConsumerEntity>(Client);
            var athletes = await service.AggregateAsync<PersonInfo>(Guid.Parse("6d2bf19f-1296-4ab8-9301-923f94eeb2bf"), new AggregateOptions
            {
                Tokens = new Dictionary<string, string>()
                {
                       { "id", coachId },
                }
            });

            return athletes;

        }

        public async Task<List<PersonInfo>> GetAllInvitersAggregate(string consumerId)
        {
            var service = new CodeMashRepository<ConsumerEntity>(Client);
            var athletes = await service.AggregateAsync<PersonInfo>(Guid.Parse("5d94a264-2ea2-4fa1-835b-01ba39b4be7d"), new AggregateOptions
            {
                Tokens = new Dictionary<string, string>()
                {
                       { "id", consumerId },
                }
            });
            return athletes;
        }

        public async Task<List<PersonInfo>> FindOutIfAthleteHasCoachAggregate(string consumerId)
        {
            var service = new CodeMashRepository<ConsumerEntity>(Client);
            var athletes = await service.AggregateAsync<PersonInfo>(Guid.Parse("d2f01ec2-cafc-416a-9edc-c27368c24908"), new AggregateOptions
            {
                Tokens = new Dictionary<string, string>()
                {
                       { "id", consumerId },
                }
            });
            return athletes;
        }

        public async Task<List<PersonInfo>> GetUserFriendsAggregate(string consumerId)
        {
            var service = new CodeMashRepository<ConsumerEntity>(Client);
            var athletes = await service.AggregateAsync<PersonInfo>(Guid.Parse("5b5a72e1-ecd6-4dc0-8f9e-4a859a166e85"), new AggregateOptions
            {
                Tokens = new Dictionary<string, string>()
                {
                       { "id", consumerId },
                }
            });
            return athletes;
        }
    }
}
