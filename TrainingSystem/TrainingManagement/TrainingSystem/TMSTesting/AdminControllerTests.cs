using NUnit.Framework;
using NUnit.Framework.Internal;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using TMS;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Contracts.Repositories.InternalManagement;
using TMS.Contracts.Services.InternalManagement;
using TMS.Controllers.InternalManagement;

namespace TMSTesting
{
    public class AdminControllersTests
    {       
               
        public IConsumerRepository ConsumerRepositoryMock;
        public IAuthService AuthServiceMock;
        public AdminController AdminControllerMock;
        public ConsumerEntity newUser;
        public List<ConsumerEntity> users;
        public ClaimsPrincipal user;
        [SetUp]
        public void Setup()
        {
            ConsumerRepositoryMock = Substitute.For<IConsumerRepository>();
            AuthServiceMock = Substitute.For<IAuthService>();
            AdminControllerMock = new AdminController()
            {
                ConsumerRepository = ConsumerRepositoryMock,
                authService = AuthServiceMock
            };

             user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
              {
                 new Claim(ClaimTypes.NameIdentifier, "testId"),
                 new Claim(ClaimTypes.Name, "testName"),
                 new Claim(ClaimTypes.Role, "Admin")
              }));
            newUser = new ConsumerEntity()
            {
                Name = "Ern",
                Surname = "Kar",
                Athletes = new List<string>(),
                Competitions = new List<CompetitionEntity> { },
                Email = "ern@gmail.com",
                EmailConfirmed = true,
                Friends = new List<string>(),
                Id = "5e7b9c1f9805e2000126e82e",
                InviteFrom = new List<string>(),
                Password = "aurimas1997",
                Records = new List<CompetitionEntity> { },
                Role = "Coach",
                Salt = "0iiWScc3aka3f9MjTm21cQ=="
            };

        }
               

       

        [Test]
        public async Task PostTraining_success_Test()
        {

            AdminControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            ConsumerRepositoryMock.GetAllUsers().Returns(users);

            // Act

            var actionResult = await AdminControllerMock.GetUsers();
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task InsertUser_BadRequest_password_Test()
        {

            AdminControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            newUser.Password = "aa";

            // Act

            var actionResult = await AdminControllerMock.InsertUser(newUser);
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }
        [Test]
        public async Task InsertUser_BadRequest_Test()
        {

            AdminControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange         
            AuthServiceMock.InsertNewAdmin(Arg.Any<ConsumerEntity>()).Returns("yra");
            // Act

            var actionResult = await AdminControllerMock.InsertUser(newUser);
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }
        [Test]
        public async Task InsertUser_success_Test()
        {

            AdminControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange         
            AuthServiceMock.InsertNewAdmin(Arg.Any<ConsumerEntity>()).Returns((string)null);
            // Act

            var actionResult = await AdminControllerMock.InsertUser(newUser);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task DeleteUser_success_Test()
        {

            AdminControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange         
            ConsumerRepositoryMock.GetAllUsers().Returns(users);
            // Act

            var actionResult = await AdminControllerMock.DeleteUser("id");
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }
    }
}





