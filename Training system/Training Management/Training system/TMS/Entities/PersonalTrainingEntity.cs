using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    [Collection("personal-trainings")]
    public class PersonalTrainingEntity:Entity
    {
        [Field("day")]
        public DateTime Day { get; set; }
        [Field("training")]
        public string TrainTemplateId { get; set; }
        [Field("results")]
        public List<SetEntity> Results { get; set; } = new List<SetEntity>();
        [Field("athlete")]
        public string AthleteId { get; set; }
        [Field("coach")]
        public string CoachId { get; set; }
        [Field("athletereport")]
        public string AthleteReport { get; set; } = "";
        [Field("description")]
        public string Description { get; set; }
    }
}
