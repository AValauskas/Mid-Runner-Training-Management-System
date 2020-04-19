using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public interface IAuthRepository
    {
        Task RegisterUser(User user);
        Task<string> CheckIfEmailAlreadyExist(User user);
        Task LoginUser(string email, string password);
        Task ChangePassword(string ConsumerId, HashPasswordInfo hashedInfo);
        Task VerifyRegister(string ConsumerId);
    }
}
