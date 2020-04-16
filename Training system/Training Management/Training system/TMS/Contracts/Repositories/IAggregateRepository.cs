using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public interface IAggregateRepository
    {
        Task<List<CoachAssignedTrains>> GetCoachAssignedTrainingsCount(string coachId);
    }
}
