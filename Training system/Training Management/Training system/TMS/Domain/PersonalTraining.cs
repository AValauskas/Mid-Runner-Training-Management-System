using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class PersonalTraining
    {
        
        public DateTime Day { get; set; } = DateTime.Now;
     
        public string TrainTemplateId { get; set; } = "";
       
        
        public List<string> AthleteIds { get; set; } = new List<string>();
     
        public string CoachId { get; set; } = "";
       
       
        public string Description { get; set; } = "";

        public string Place { get; set; } = "";
    }
}
