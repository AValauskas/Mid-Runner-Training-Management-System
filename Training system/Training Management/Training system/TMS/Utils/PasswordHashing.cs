using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TMS
{
    public static class PasswordHashing
    {
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
    }
}
