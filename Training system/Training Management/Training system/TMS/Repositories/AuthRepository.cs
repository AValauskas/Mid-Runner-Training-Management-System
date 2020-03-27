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


            /* if (user.Role == "Athlete")
             {
                 var registerService = new CodeMashRepository<AthleteEntity>(Client);

                 var consumer = new AthleteEntity()
                 {
                     Email = user.Email,
                     Name = user.Name,
                     Password = user.Password,
                     Surname = user.Surname
                 };
                 await registerService.InsertOneAsync(consumer);
             }
             else if (user.Role == "Coach")
             {
                 var registerService = new CodeMashRepository<CoachEntity>(Client);

                 var consumer = new CoachEntity()
                 {
                     Email = user.Email,
                     Name = user.Name,
                     Password = user.Password,
                     Surname = user.Surname                    
             };
                 await registerService.InsertOneAsync(consumer);
             }   */
        }

        public async Task<string> CheckIfPasswordOrEmailAlreadyExist(User user)
        {
            var registerService = new CodeMashRepository<ConsumerEntity>(Client);
            var email = await registerService.FindOneAsync(x => x.Email == user.Email);
            var password = await registerService.FindOneAsync(x => x.Password == user.Password);

            if (email != null)
            {
                return "email already exist";
            }
            else if (password != null)
            {
                return "password already exist";
            }
            /*
            var registerServiceAthlete = new CodeMashRepository<AthleteEntity>(Client);
            var athleteEmail = await registerServiceAthlete.FindOneAsync(x => x.Email == user.Email);
            var athletePassword= await registerServiceAthlete.FindOneAsync(x => x.Password == user.Password);

            var registerServiceCoach= new CodeMashRepository<CoachEntity>(Client);
            var coachEmail = await registerServiceAthlete.FindOneAsync(x => x.Email == user.Email);
            var coachPassword = await registerServiceAthlete.FindOneAsync(x => x.Email == user.Email);

            if (athleteEmail != null)
            {
                return "email already exist";
            }
            else if (athletePassword != null)
            {
                return "password already exist";
            }
            else if (athletePassword != null)
            {
                return "email already exist";
            }else if (athletePassword != null)
            {
                return "password already exist";
            }*/
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
