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

    }
}
