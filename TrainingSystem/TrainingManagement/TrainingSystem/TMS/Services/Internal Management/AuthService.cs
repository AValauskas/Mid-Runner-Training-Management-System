using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TMS
{
    public class AuthService: IAuthService
    {
        public IAuthRepository AuthRepository { get; set; }
        public IConsumerRepository ConsumerRepository { get; set; }

        public IEmailRepository EmailRepository { get; set; }
        public async Task<string> Register(ConsumerEntity user)
        {
            if (user.Password.Length<6)
            {
                throw new Exception("Password must be atleast 6 symbols length");
            }
            if (!Regex.IsMatch(user.Password, @"^(?=.*[a-zA-Z])(?=.*[0-9])"))
            {
                throw new Exception("Password must contain atleast one number and atleast one letter");

            }
            var response = await AuthRepository.CheckIfEmailAlreadyExist(user);
            if(response != null)
            {
                return response;
            }

            var hash = HashNewPassword(user.Password);
            user.Password = hash.Password;
            user.Salt = hash.Salt;
            var consumer = await AuthRepository.RegisterUser(user);
            await EmailRepository.SendEmailConfirmationEmail(user.Email, consumer.Id);
            return null;
        }                     
        

        public async Task<JwtSecurityToken> Login(ConsumerEntity user)
        {
            
           var consumer = await ConsumerRepository.FindConsumerByEmail(user.Email);
            if (consumer!=null)
            {           
            if (!consumer.EmailConfirmed)
            {
                throw new Exception("email isn't confirmed yet");
            }
            var hash = HashOldPassword(user.Password, consumer.Salt);         

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
        }


        public async Task ChangePassword(string idConsumer, string password)
        {

            if (password.Length < 6)
            {
                throw new Exception( "Password is too short");
            }
            if (!Regex.IsMatch(password, @"^(?=.*[a-zA-Z])(?=.*[0-9])"))
            {
                throw new Exception("Password must contain atleast one number and atleast one letter");                
            }

            var hashedPassword = HashNewPassword(password);
            await AuthRepository.ChangePassword(idConsumer, hashedPassword);
        }

        public async Task<string> RequestForNewPassword(string email)
        {
           var consumer = await ConsumerRepository.FindConsumerByEmail(email);
            if (consumer!=null)
            {
                await EmailRepository.SendPasswordResetEmail(email, consumer.Id);
                return null;
            }
            return "email not exist";

        }

        public async Task ResetPassword(string idConsumer, string password)
        {

            var consumer = await ConsumerRepository.FindConsumerById(idConsumer);
            var hashedPassword = HashNewPassword(password);
            await AuthRepository.ChangePassword(idConsumer, hashedPassword);
           // var consumer = await ConsumerRepository.FindConsumerById(idConsumer);
            await EmailRepository.SendNewPassword(consumer.Email, password);
        }


        public static HashPasswordInfo HashNewPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashedPass = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );
            var hashPw = new HashPasswordInfo() { Password = hashedPass, Salt = Convert.ToBase64String(salt) };
            return hashPw;
        }
        public static HashPasswordInfo HashOldPassword(string password, string saltString)
        {
            byte[] salt = Convert.FromBase64String(saltString);
            string hashedPass = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );
            var hashPw = new HashPasswordInfo() { Password = hashedPass, Salt = Convert.ToBase64String(salt) };
            return hashPw;

        }

        public async Task<string> InsertNewAdmin(ConsumerEntity user)
        {
            if (!Regex.IsMatch(user.Password, @"^(?=.*[a-zA-Z])(?=.*[0-9])"))
            {
                throw new Exception("Password must contain atleast one number and atleast one letter");

            }
            var response = await AuthRepository.CheckIfEmailAlreadyExist(user);
            if (response != null)
            {
                return response;
            }

            var hash = HashNewPassword(user.Password);
            user.Password = hash.Password;
            user.Salt = hash.Salt;
            user.EmailConfirmed = true;
            var consumer = await AuthRepository.RegisterUser(user);          
            return null;
        }
    }
}
