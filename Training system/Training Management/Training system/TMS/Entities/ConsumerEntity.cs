using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    [Collection("consumers")]
    public class ConsumerEntity:Entity
    {
        [Field("name")]
        public string Name { get; set; } = "";
        [Field("surname")]
        public string Surname { get; set; } = "";
        [Field("competitions")]
        public List<CompetitionEntity> Competitions { get; set; } = new List<CompetitionEntity>();
        [Field("records")]
        public List<CompetitionEntity> Records { get; set; } = new List<CompetitionEntity>();
        [Field("email")]
        public string Email { get; set; } = "";
        [Field("password")]
        public string Password { get; set; } = "";
        [Field("role")]
        public string Role { get; set; } = "";
        [Field("athletes")]
        public List<string> Athletes { get; set; } = new List<string>();
        [Field("invitefrom")]
        public List<string> InviteFrom { get; set; } = new List<string>();
    }
}
