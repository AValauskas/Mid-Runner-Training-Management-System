using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Training_Management
{
    public class AuthService: IAuthService
    {
        public IAuthRepository AuthRepository { get; set; }
        public IAthleteRepository AthleteRepository { get; set; }
        public ICoachRepository CoachRepository { get; set; }
        public async Task Register(User user)
        {
            if (user.Password.Length<6)
            {
                throw new Exception("Password is too short");
            }
            if (!user.Password.Any(char.IsDigit))
            {
                throw new Exception("Password must contain atleast one number");
            }

            await AuthRepository.RegisterUser(user.Email, user.Password, user.Role);

        }









        public JwtSecurityToken Login(User user)
        {
            DoExistLogin(ref user);

            if (user.Id!=null)
            {
                if (user.PersonPosition == "Athlete")
                {
                   return GenerateAthleteToken(user.Id);
                }
                if (user.PersonPosition == "Coach")
                {
                    return GenerateCoachToken(user.Id);
                }
            }
            return null;

        }


        public JwtSecurityToken GenerateAthleteToken(string id)
        {
            Environment.SetEnvironmentVariable("TestAthlete", "this_is_Athlete_key");

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TestLecturer")));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            // add claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "Athlete"));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, id));

            //create token
            var token = new JwtSecurityToken(
                issuer: "Training-management.lt",
                audience: "readers",
                expires: DateTime.Now.AddHours(3),
                signingCredentials: signingCredentials,
                claims: claims

                );
            return token;
            // return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public JwtSecurityToken GenerateCoachToken(string id)
        {
            Environment.SetEnvironmentVariable("TestCoach", "this_is_Coach_key");

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TestCoach")));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            // add claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "Coach"));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, id));

            //create token
            var token = new JwtSecurityToken(
                issuer: "Training-management.lt",
                audience: "readers",
                expires: DateTime.Now.AddHours(3),
                signingCredentials: signingCredentials,
                claims: claims
                );
            return token;
            // return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }


        public void DoExistLogin(ref User user)
        {
            var athlete = AthleteRepository.FindAthlete(user);
             if (athlete!=null)
             {
                user.Id = athlete.Id;
                user.PersonPosition = "Athlete";
             }
            var coach = CoachRepository.FindCoach(user).Result;
            if (coach != null)
            {
                user.Id = coach.Id;
                user.PersonPosition = "Coach";
            }

        }



    }
}
