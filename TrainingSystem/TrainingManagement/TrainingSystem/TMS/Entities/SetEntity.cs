using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class SetEntity
    {
        [Field("distance")]
        public double Distance { get;set; }
        [Field("pace")]
        public double Pace { get; set; }
        [Field("rest")]
        public double Rest { get; set; }
    }
}
