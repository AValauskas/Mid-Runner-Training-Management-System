using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS.Contracts.Repositories.InternalManagement
{
    public interface IAuthRepository
    {
        Task<ConsumerEntity> RegisterUser(ConsumerEntity user);
        Task<string> CheckIfEmailAlreadyExist(ConsumerEntity user);
        Task ChangePassword(string consumerId, HashPasswordInfo hashedInfo);
        Task VerifyRegister(string consumerId);
    }
}
