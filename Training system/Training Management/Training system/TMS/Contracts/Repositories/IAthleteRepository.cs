using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public interface IAthleteRepository
    {
        Task<AthleteEntity> FindAthlete(User user);
    }
}
