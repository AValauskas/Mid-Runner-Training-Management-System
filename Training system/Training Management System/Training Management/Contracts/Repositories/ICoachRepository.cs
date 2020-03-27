using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Training_Management
{
    public interface ICoachRepository
    {
        Task<CoachEntity> FindCoach(User user);
    }
}
