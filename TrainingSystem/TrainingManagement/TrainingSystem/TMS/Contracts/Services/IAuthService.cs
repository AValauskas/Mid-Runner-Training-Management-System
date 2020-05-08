using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
   public interface IAuthService
    {
        Task<string> Register(ConsumerEntity user);
        Task<string> InsertNewAdmin(ConsumerEntity user);
        Task<JwtSecurityToken> Login(ConsumerEntity user);

        JwtSecurityToken GenerateCoachToken(string id);
        JwtSecurityToken GenerateAthleteToken(string id);
        Task ChangePassword(string idConsumer, string password);
        Task<string> RequestForNewPassword(string email);
        Task ResetPassword(string idConsumer, string password);
    }
}
