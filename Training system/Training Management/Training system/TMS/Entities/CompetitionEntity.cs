using CodeMash.Models;
using System;

namespace TMS
{
    public class CompetitionEntity
    {
        [Field("distance")]
        public int Distance { get; set; } = 0;
        [Field("date")]
        public DateTime Date { get; set; } = DateTime.Now;
        [Field("time")]
        public double Time { get; set; } = 0;
        [Field("competition_name")]
        public string CompetitionName { get; set; } = "";
        [Field("place")]
        public string Place { get; set; } = InsideOutside.Inside.ToString();
    }
}