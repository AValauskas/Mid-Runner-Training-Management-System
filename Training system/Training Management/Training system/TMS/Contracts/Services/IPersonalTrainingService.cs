﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public interface IPersonalTrainingService
    {
        Task ProcessPersonalTraining(PersonalTraining training);
    }
}
