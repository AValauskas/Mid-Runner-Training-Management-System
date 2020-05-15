﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public interface IAuthRepository
    {
        Task<ConsumerEntity> RegisterUser(ConsumerEntity user);
        Task<string> CheckIfEmailAlreadyExist(ConsumerEntity user);
        Task ChangePassword(string ConsumerId, HashPasswordInfo hashedInfo);
        Task VerifyRegister(string ConsumerId);
    }
}