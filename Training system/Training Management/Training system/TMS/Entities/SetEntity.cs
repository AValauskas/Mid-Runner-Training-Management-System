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
        public int Distance { get;set; }
        [Field("pace")]
        public int Pace { get; set; }
        [Field("rest")]
        public int Rest { get; set; }
    }
}
