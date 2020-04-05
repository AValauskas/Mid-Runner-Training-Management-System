using CodeMash.Client;
using CodeMash.Membership.Services;
using CodeMash.Repository;
using Isidos.CodeMash.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class AuthRepository: IAuthRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);
        public async Task RegisterUser(User user)
        {
            var registerService = new CodeMashRepository<ConsumerEntity>(Client);

            var consumer = new ConsumerEntity()
            {
                Email = user.Email,
                Name = user.Name,
                Password = user.Password,
                Surname = user.Surname,
                Role =user.Role
            };
            await registerService.InsertOneAsync(consumer);


        }

        public async Task<string> CheckIfEmailAlreadyExist(User user)
        {
            var registerService = new CodeMashRepository<ConsumerEntity>(Client);
            var email = await registerService.FindOneAsync(x => x.Email == user.Email);

            if (email != null)
            {
                return "email already exist";
            }

            return null;
        }

            public async Task LoginUser(string email, string password)
        {
            // 3. Create a service object
            var membershipService = new CodeMashMembershipService(Client);

           var result = await membershipService.AuthenticateCredentialsAsync(
            email,
            password,
            permanentSession: true
            );
            //return result;
        }
    }
}
