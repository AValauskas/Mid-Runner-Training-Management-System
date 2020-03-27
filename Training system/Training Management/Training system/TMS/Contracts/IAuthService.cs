using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
   public interface IAuthService
    {
        Task<string> Register(User user);
        Task<JwtSecurityToken> Login(User user);

        JwtSecurityToken GenerateCoachToken(string id);
        JwtSecurityToken GenerateAthleteToken(string id);
    }
}
