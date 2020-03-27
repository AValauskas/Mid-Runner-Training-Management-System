﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Training_Management
{
    public interface IAuthRepository
    {
        Task RegisterUser(string email, string password, string role);
        Task LoginUser(string email, string password);
    }
}