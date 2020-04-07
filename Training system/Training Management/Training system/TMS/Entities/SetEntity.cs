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
        public float Distance { get;set; }
        [Field("pace")]
        public float Pace { get; set; }
        [Field("rest")]
        public float Rest { get; set; }
    }
}
