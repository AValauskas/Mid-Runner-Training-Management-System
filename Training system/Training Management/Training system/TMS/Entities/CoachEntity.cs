﻿using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    [Collection("Trainers")]
    public class CoachEntity:Entity
    {
        [Field("name")]
        public string Name { get; set; }
        [Field("surname")]
        public string Surname { get; set; }
        [Field("email")]
        public string Email { get; set; }
        [Field("password")]
        public string Password { get; set; }
    }
}