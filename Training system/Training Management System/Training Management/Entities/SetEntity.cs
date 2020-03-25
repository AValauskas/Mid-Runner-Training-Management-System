﻿using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Training_Management
{
    public class SetEntity
    {
        [Field("distance")]
        public int Distance { get;set; }
        [Field("pace")]
        public int Pace { get; set; }
        [Field("rest")]
        public string Rest { get; set; }
    }
}
