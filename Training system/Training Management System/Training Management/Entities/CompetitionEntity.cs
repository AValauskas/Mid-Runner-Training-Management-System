using CodeMash.Models;
using System;

namespace Training_Management
{
    public class CompetitionEntity
    {
        [Field("distance")]
        public int Distance { get; set; }
        [Field("besttimedate")]
        public DateTime BestTimeDate { get; set; }
        [Field("besttime")]
        public string BestTime { get; set; }
    }
}