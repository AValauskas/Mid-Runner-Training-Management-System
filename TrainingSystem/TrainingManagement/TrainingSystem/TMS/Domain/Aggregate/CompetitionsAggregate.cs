using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class CompetitionsAggregate
    {
        [Field("competitions")]
        public List<Record> Records { get; set; }
    }
}
