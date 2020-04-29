using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class TrainingsWhichAreAssignedByDate
    {
        [Field("athlete")]
        public string Athlete { get; set; }
        [Field("athleteId")]
        public string AthleteId { get; set; }
        [Field("definition")]
        public string Definition { get; set; }
        [Field("trainingId")]
        public string TrainingTemplateId { get; set; }
        [Field("_id")]
        public string PersonalTrainingId { get; set; }
    }
}
