using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using LambdaFunction.Inputs;
using LambdaFunction.Services;
using LambdaFunction.Settings;
using Training_Management;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaFunction
{
    public class Function
    {
        private readonly IExampleService _exampleService;
        
        // (Required if adding other constructors. Otherwise, optional.) A default constructor
        // called by Lambda. If you are adding your custom constructors,
        // default constructor with no parameters must be added
        public Function() : this (new ExampleService()) {}

        // (Optional) An example of injecting a service. As a default constructor is called by Lambda
        // this constructor has to be called from default constructor
        public Function(IExampleService exampleService)
        {
            _exampleService = exampleService;
        }
        
        /// <summary>
        /// (Required) Entry method of your Lambda function.
        /// </summary>
        /// <param name="lambdaEvent">Type returned from CodeMash</param>
        /// <param name="context">Context data of a function (function config)</param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> Handler(CustomEventRequest<BasicInput> lambdaEvent, ILambdaContext context)
        {
            string email, password, role;
             User user;
            /* 
             if (lambdaEvent.Input.Data != null)
             {
                 if (lambdaEvent.Input.Data != "")
                 {
                     user = JsonConvert.DeserializeObject<User>(lambdaEvent.Input.Data);
                 }
                 else
                 {
                     email = Environment.GetEnvironmentVariable("email");
                     password = Environment.GetEnvironmentVariable("password");
                     role = Environment.GetEnvironmentVariable("role");
                     user = new User()
                     {
                         Email = email,
                         Password = password,
                         PersonPosition = role
                     };
                 }
             }
             else
             {
                 email = Environment.GetEnvironmentVariable("email");
                 password = Environment.GetEnvironmentVariable("password");
                 role = Environment.GetEnvironmentVariable("role");
                 user = new User()
                 {
                     Email = email,
                     Password = password,
                     PersonPosition = role
                 };
             }*/

            user = new User()
            {
                Email = "ab@gmail.com",
                Password = "aaaaaaa11a",
                Role = "Athlete"
            };


            if (Environment.GetEnvironmentVariable("ApiKey") != null)
            {
                Training_Management.Settings.ApiKey = Environment.GetEnvironmentVariable("ApiKey");
            }

            IAuthService authService = new AuthService()
            {
                AuthRepository = new AuthRepository(),
                
            };
            await authService.Register(user);

            var response = new
            {
                lambdaEvent,
            };
            
            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(response),
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}