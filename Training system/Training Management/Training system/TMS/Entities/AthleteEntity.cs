using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    [Collection("athletes")]
    public class AthleteEntity:Entity
    {
        [Field("name")]
        public string Name { get; set; }
        [Field("surname")]
        public string Surname { get; set; }
        [Field("competition")]
        public List<CompetitionEntity> Competitions { get; set; } = new List<CompetitionEntity>();
        [Field("coach")]
        public string Coach { get; set; }
        [Field("username")]
        public string Username { get; set; }
        [Field("password")]
        public string Password { get; set; }
    }
}
