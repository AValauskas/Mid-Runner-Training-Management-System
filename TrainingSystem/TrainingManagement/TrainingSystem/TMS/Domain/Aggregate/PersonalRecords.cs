using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class PersonalRecords
    {
        [Field("records")]
        public List<Record> Records { get; set; } 
    }
    public class Record
    {
        [Field("distance")]
        public string Distance { get; set; }
        [Field("date")]
        public DateTime Date { get; set; }
        [Field("time")]
        public double Time { get; set; }
        [Field("competition_name")]
        public string Competition { get; set; }
        [Field("place")]
        public string Place { get; set; }
        
        public string DateString { get; set; }
    }

}
