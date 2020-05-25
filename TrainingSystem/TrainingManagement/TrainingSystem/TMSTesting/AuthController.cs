using NUnit.Framework;
using NUnit.Framework.Internal;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using TMS;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using TMS.Contracts.Repositories.InternalManagement;
using TMS.Contracts.Services.InternalManagement;
using TMS.Controllers.InternalManagement;

namespace TMSTesting
{
    public class AuthControllerTests
    {    
        public IAuthRepository AuthRepositoryMock;
        public IAuthService AuthServiceMock;
        public AuthController AuthControllerMock;
        public ConsumerEntity athlete;
        public ConsumerEntity newUser;

        [SetUp]
        public void Setup()
        {

            AuthRepositoryMock = Substitute.For<IAuthRepository>();
            AuthServiceMock = Substitute.For<IAuthService>();
            AuthControllerMock = new AuthController()
            {
                authRepo = AuthRepositoryMock,
                authService = AuthServiceMock

            };
            athlete = new ConsumerEntity()
            {
                Name = "Aurimas",
                Surname = "Valauskas",
                Athletes = new List<string>(),
                Competitions = new List<CompetitionEntity> { },
                Email = "aurimas19970704@gmail.com",
                EmailConfirmed = true,
                Friends = new List<string>(),
                Id= "5e7b9bd0a04cca0001ffd0c2",
                InviteFrom= new List<string>(),
                Password= "izgZDGqCYiqLTdNrWX19KjkzGjuRdBPVmPKbZQv9XVI=",
                Records= new List<CompetitionEntity> { },
                Role="Athlete",
                Salt= "W3EuL37eopFmwMB+q/rHQg=="
            };
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
        public async Task Login_BadRequest_Test()
        {
            // Arrange
            AuthServiceMock.Login(Arg.Any<ConsumerEntity>()).Returns((JwtSecurityToken)null);

            // Act

            var actionResult = await AuthControllerMock.Login(newUser);
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }



        [Test]
        public async Task Login_success_Test()
        {
            // Arrange
            AuthServiceMock.Login(Arg.Any<ConsumerEntity>()).Returns(new JwtSecurityToken());

            // Act

            var actionResult = await AuthControllerMock.Login(newUser);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }


        [Test]
        public async Task Register_BadRequest_Test()
        {
            // Arrange
            AuthServiceMock.Register(Arg.Any<ConsumerEntity>()).Returns("error");

            // Act

            var actionResult = await AuthControllerMock.Register(newUser);
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }

        [Test]
        public async Task Register_success_Test()
        {
            // Arrange
            AuthServiceMock.Register(Arg.Any<ConsumerEntity>()).Returns((string)null);

            // Act

            var actionResult = await AuthControllerMock.Register(newUser);
            // Assert
            Assert.IsInstanceOf<OkResult>(actionResult);
        }

        [Test]
        public async Task CompleteRegister_BadRequest_Test()
        {

            // Act

            var actionResult = await AuthControllerMock.CompleteRegister("id");
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }

        [Test]
        public async Task CompleteRegister_success_Test()
        {

            // Act
            var actionResult = await AuthControllerMock.CompleteRegister("5e7b9bd0a04cca0001ffd0c2");
            // Assert
            Assert.IsInstanceOf<OkResult>(actionResult);
            await AuthRepositoryMock.Received().VerifyRegister(Arg.Any<string>());
        }
        [Test]
        public async Task RequestForNewPassword_BadRequest_Test()
        {
            // Arrange
            AuthServiceMock.RequestForNewPassword(Arg.Any<string>()).Returns("error");

            // Act

            var actionResult = await AuthControllerMock.RequestForNewPassword("email");
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }
        [Test]
        public async Task RequestForNewPassword_success_Test()
        {
            // Arrange
            AuthServiceMock.RequestForNewPassword(Arg.Any<string>()).Returns((string)null);

            // Act

            var actionResult = await AuthControllerMock.RequestForNewPassword("email");
            // Assert
            Assert.IsInstanceOf<OkResult>(actionResult);
        }


        [Test]
        public async Task ConfirmPasswordReset_BadRequest_Test()
        {
            // Arrange
            AuthServiceMock.RequestForNewPassword(Arg.Any<string>()).Returns("error");

            // Act

            var actionResult = await AuthControllerMock.ConfirmPasswordReset("id");
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }
        [Test]
        public async Task ConfirmPasswordReset_success_Test()
        {
            // Arrange
            AuthServiceMock.RequestForNewPassword(Arg.Any<string>()).Returns((string)null);

            // Act

            var actionResult = await AuthControllerMock.ConfirmPasswordReset("5e7b9bd0a04cca0001ffd0c2");
            // Assert
            Assert.IsInstanceOf<OkResult>(actionResult);
        }
    }
}





