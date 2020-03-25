using CodeMash.Client;
using CodeMash.Membership.Services;
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
        public Task RegisterUser(string email, string password, string role)
        {
            // 3. Create a service object
            var membershipService = new CodeMashMembershipService(Client);

            // 4. Call an API method
           var result = membershipService.RegisterUser(new RegisterUserRequest
            {
                Email = email,
                Password = password,
                Roles = new List<string>(){ role },
            });
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
