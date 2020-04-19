using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TMS
{
    public class AuthService: IAuthService
    {
        public IAuthRepository AuthRepository { get; set; }
        public IConsumerRepository ConsumerRepository { get; set; }
        public async Task<string> Register(User user)
        {
            if (user.Password.Length<6)
            {
                return "Password is too short";
            }
            if (!user.Password.Any(char.IsDigit))
            {
                return "Password must contain atleast one number";
            }
            //var pwd = new PasswordHash("some pwd");
            var response = await AuthRepository.CheckIfEmailAlreadyExist(user);
            if(response != null)
            {
                return response;
            }

            var hash = PasswordHashing.HashNewPassword(user.Password);
            user.Password = hash.Password;
            user.Salt = hash.Salt;
            await AuthRepository.RegisterUser(user);

            return null;
        }                     
        

        public async Task<JwtSecurityToken> Login(User user)
        {
            
           var consumer = await ConsumerRepository.FindConsumerByEmail(user.Email);
            if (!consumer.EmailConfirmed)
            {
                throw new Exception("email isn't confirmed yet");
            }
            var hash = PasswordHashing.HashOldPassword(user.Password, consumer.Salt);         

            if (hash.Password == consumer.Password)
            {
                
                if (consumer.Role == "Athlete")
                {
                   return GenerateAthleteToken(consumer.Id);
                }
                if (consumer.Role == "Coach")
                {
                    return GenerateCoachToken(consumer.Id);
                }
                if (consumer.Role == "Admin")
                {
                    return GenerateAdminToken(consumer.Id);
                }
            }
            return null;
        }

        public JwtSecurityToken GenerateAthleteToken(string id)
        {
            Environment.SetEnvironmentVariable("TestAthlete", "this_is_Athlete_key");

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TestAthlete")));
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
        public JwtSecurityToken GenerateAdminToken(string id)
        {
            Environment.SetEnvironmentVariable("TestAdmin", "this_is_Admin_key");

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TestAdmin")));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            // add claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
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

        public async Task<User> DoExistLogin(User user)
        {
            var consumer = await ConsumerRepository.FindConsumer(user);
             if (consumer != null)
             {
                user.Id = consumer.Id;
                user.Role = consumer.Role;
             }

            return user;

        }


        public async Task ChangePassword(string idConsumer, string password)
        {
            var hashedPassword = PasswordHashing.HashNewPassword(password);
            await AuthRepository.ChangePassword(idConsumer, hashedPassword);

        }
    }
}
