using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class CoachAssignedTrains
    {
        [Field("description")]
        public string Description { get; set; }
        [Field("day")]
        public DateTime Day { get; set; }
    }
}
