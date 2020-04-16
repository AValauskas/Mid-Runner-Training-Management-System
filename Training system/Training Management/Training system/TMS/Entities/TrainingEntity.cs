using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    [Collection("Trainings")]
    public class TrainingEntity:Entity
    {
        [Field("description")]
        public string Description { get; set; }
        [Field("repeats")]
        public int Repeats { get; set; }
        [Field("set")]
        public List<SetEntity> Sets { get; set; } = new List<SetEntity>();
        [Field("level")]
        public int Level { get; set; }
        [Field("destinition")]
        public int Destinition { get; set; }
        [Field("seasontime")]
        public string SeasonTime { get; set; }
        [Field("trainingtype")]
        public string TrainingType { get; set; }
        [Field("owner")]
        public string Owner { get; set; }
        [Field("personal")]
        public bool IsPersonal { get; set; }
        [Field("description_to_display")]
        public string ToDisplay { get; set; }

    }
}
