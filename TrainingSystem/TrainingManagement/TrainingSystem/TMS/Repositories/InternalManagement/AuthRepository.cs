using CodeMash.Client;
using CodeMash.Membership.Services;
using CodeMash.Repository;
using Isidos.CodeMash.ServiceContracts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMS.Contracts.Repositories.InternalManagement;

namespace TMS.Repositories.InternalManagement
{
    public class AuthRepository: IAuthRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);
        public async Task<ConsumerEntity> RegisterUser(ConsumerEntity consumer)
        {
            var registerService = new CodeMashRepository<ConsumerEntity>(Client);
            consumer = await registerService.InsertOneAsync(consumer);

            return consumer;
        }

        public async Task<string> CheckIfEmailAlreadyExist(ConsumerEntity user)
        {
            var registerService = new CodeMashRepository<ConsumerEntity>(Client);
            var email = await registerService.FindOneAsync(x => x.Email == user.Email);

            if (email != null)
            {
                return "email already exist";
            }

            return null;
        }


        public async Task ChangePassword(string ConsumerId, HashPasswordInfo hashedInfo)
        {
            // 3. Create a service object
            var membershipService = new CodeMashMembershipService(Client);

            var ConsumerRepository = new CodeMashRepository<ConsumerEntity>(Client);

            var updateBuilder = Builders<ConsumerEntity>.Update;
            var update = updateBuilder.Set(x => x.Password, hashedInfo.Password).Set(x => x.Salt, hashedInfo.Salt);

            var result = await ConsumerRepository.UpdateOneAsync(x => x.Id == ConsumerId, update);
            //return result;
        }

        public async Task VerifyRegister(string ConsumerId)
        {
            // 3. Create a service object
            var membershipService = new CodeMashMembershipService(Client);

            var ConsumerRepository = new CodeMashRepository<ConsumerEntity>(Client);

            var updateBuilder = Builders<ConsumerEntity>.Update;
            var update = updateBuilder.Set(x => x.EmailConfirmed, true);
            var result = await ConsumerRepository.UpdateOneAsync(x => x.Id == ConsumerId, update);
            //return result;
        }
    }
}
