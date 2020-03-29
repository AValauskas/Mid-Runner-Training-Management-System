using CodeMash.Models;
using System;

namespace TMS
{
    public class CompetitionEntity
    {
        [Field("distance")]
        public int Distance { get; set; }
        [Field("date")]
        public DateTime Date { get; set; }
        [Field("time")]
        public double Time { get; set; }
    }
}