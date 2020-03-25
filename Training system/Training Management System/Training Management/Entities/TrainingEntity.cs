using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Training_Management
{
    [Collection("Trainings")]
    public class TrainingEntity:Entity
    {
        [Field("description")]
        public string Description { get; set; }
        [Field("repeats")]
        public int Repeats { get; set; }
        [Field("sets")]
        public List<SetEntity> Sets { get; set; } = new List<SetEntity>();
        [Field("level")]
        public int Level { get; set; }
        [Field("seasontime")]
        public string SeasonTime { get; set; }
        [Field("trainingtype")]
        public string TrainingType { get; set; }
    
    }
}
