using NUnit.Framework;
using NUnit.Framework.Internal;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using TMS;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace TMSTesting
{
    public class AuthTests
    {
        public IConsumerRepository ConsumerRepository;
        public IAuthRepository AuthRepository;
        public IEmailRepository EmailRepository;
        public IAuthService AuthService;
     
          
        public IConsumerRepository ConsumerRepositoryMock;
        public IAuthRepository AuthRepositoryMock;
        public IEmailRepository EmailRepositoryMock;
        public IAuthService AuthServiceMock;
        public ConsumerEntity athlete;
        public ConsumerEntity newUser;

        [SetUp]
        public void Setup()
        {
            ConsumerRepository = new ConsumerRepository();
            AuthRepository = new AuthRepository();
            EmailRepository = new EmailRepository();
            AuthService = new AuthService()
            {
                AuthRepository= AuthRepository,
                ConsumerRepository= ConsumerRepository,
                EmailRepository= EmailRepository
            };

            ConsumerRepositoryMock = Substitute.For<IConsumerRepository>();
            AuthRepositoryMock = Substitute.For<IAuthRepository>();
            EmailRepositoryMock = Substitute.For<IEmailRepository>();
            AuthServiceMock = new AuthService()
            {
                AuthRepository = AuthRepositoryMock,
                ConsumerRepository = ConsumerRepositoryMock,
                EmailRepository = EmailRepositoryMock
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
        public async Task Register_short_password_Test()
        {
            // Arrange
            newUser.Password = "pass";

            // Act
          
            var exception = Assert.ThrowsAsync<Exception>(async () => await AuthServiceMock.Register(newUser));

            // Assert
            Assert.AreEqual("Password must be atleast 6 symbols length", exception.Message);
        }


        [Test]
        public async Task Register_only_letters_Test()
        {
            // Arrange
            newUser.Password = "passsss";

            // Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await AuthServiceMock.Register(newUser));

            // Assert
            Assert.AreEqual("Password must contain atleast one number and atleast one letter", exception.Message);
        }


        [Test]
        public async Task Register_only_numbers_Test()
        {
            // Arrange
            newUser.Password = "1234567";
            // ConsumerRepositoryMock.CheckIfRecordExist(Arg.Any<string>(), Arg.Any<CompetitionEntity>()).Returns((ConsumerEntity)null);

            // Act

            var exception = Assert.ThrowsAsync<Exception>(async () => await AuthServiceMock.Register(newUser));

            // Assert
            Assert.AreEqual("Password must contain atleast one number and atleast one letter", exception.Message);
          
        }

        [Test]
        public async Task Register_email_exist_Test()
        {
            // Arrange
            AuthRepositoryMock.CheckIfEmailAlreadyExist(Arg.Any<ConsumerEntity>()).Returns("email already exist");
            
            // Act
             var response = await AuthServiceMock.Register(newUser);

            // Assert
            Assert.AreEqual("email already exist", response);
        }

        [Test]
        public async Task Register_success_Tests()
        {
            // Arrange
            AuthRepositoryMock.CheckIfEmailAlreadyExist(Arg.Any<ConsumerEntity>()).Returns((string)null);
            AuthRepositoryMock.RegisterUser(Arg.Any<ConsumerEntity>()).Returns(newUser);

            // Act
            var response = await AuthServiceMock.Register(newUser);

            // Assert
              await EmailRepositoryMock.Received().SendEmailConfirmationEmail(Arg.Any<string>(), Arg.Any<string>());

            Assert.AreEqual(null, response);
        }

        [Test]
        public async Task Login_Email_Not_confirmed_Tests()
        {
            // Arrange
            athlete.EmailConfirmed = false;
            ConsumerRepositoryMock.FindConsumerByEmail(Arg.Any<string>()).Returns(athlete);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await AuthServiceMock.Login(newUser));

            // Assert
            Assert.AreEqual("email isn't confirmed yet", exception.Message);
        }
        [Test]
        public async Task Login_Password_not_good_Test()
        {
            // Arrange
           
            ConsumerRepositoryMock.FindConsumerByEmail(Arg.Any<string>()).Returns(newUser);

            // Act
           var response = await AuthServiceMock.Login(newUser);

            // Assert
            Assert.AreEqual(null, response);
        }
        [Test]
        public async Task Login_Success_Athlete_Test()
        {
            // Arrange

            ConsumerRepositoryMock.FindConsumerByEmail(Arg.Any<string>()).Returns(athlete);

            // Act
            var response = await AuthServiceMock.Login(newUser);

            // Assert
            Assert.NotNull(response);
        }
        [Test]
        public async Task Login_Success_Coach_Test()
        {
            // Arrange
            athlete.Role = "Coach";
            ConsumerRepositoryMock.FindConsumerByEmail(Arg.Any<string>()).Returns(athlete);

            // Act
            var response = await AuthServiceMock.Login(newUser);

            // Assert
            Assert.NotNull(response);
        }
        [Test]
        public async Task Login_Success_Admin_Test()
        {
            // Arrange
            athlete.Role = "Admin";
            ConsumerRepositoryMock.FindConsumerByEmail(Arg.Any<string>()).Returns(athlete);

            // Act
            var response = await AuthServiceMock.Login(newUser);

            // Assert
            Assert.NotNull(response);
        }

        [Test]
        public async Task ChangePassword_short_password_Test()
        {
            // Arrange
            newUser.Password = "pass";

            // Act

            var exception = Assert.ThrowsAsync<Exception>(async () => await AuthServiceMock.ChangePassword("id","pass"));

            // Assert
            Assert.AreEqual("Password is too short", exception.Message);
        }


        [Test]
        public async Task ChangePassword_only_letters_Test()
        {
            // Arrange
            newUser.Password = "passsss";

            // Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await AuthServiceMock.ChangePassword("id", "idddddd"));

            // Assert
            Assert.AreEqual("Password must contain atleast one number and atleast one letter", exception.Message);
        }

        [Test]
        public async Task ChangePassword_Success_Test()
        {

            // Act
            await AuthServiceMock.ChangePassword("id", "pass123");

            // Assert
            await AuthRepositoryMock.Received().ChangePassword(Arg.Any<string>(), Arg.Any<HashPasswordInfo>());
        }

        [Test]
        public async Task RequestForNewPassword_email_not_exist_Test()
        {
            ConsumerRepositoryMock.FindConsumerByEmail(Arg.Any<string>()).Returns((ConsumerEntity)null);
            // Act
           var response = await AuthServiceMock.RequestForNewPassword("email");

            // Assert
            Assert.AreEqual("email not exist", response);
        }

        [Test]
        public async Task RequestForNewPassword_success_Test()
        {
            ConsumerRepositoryMock.FindConsumerByEmail(Arg.Any<string>()).Returns(athlete);
            // Act
            var response = await AuthServiceMock.RequestForNewPassword("email");

            await EmailRepositoryMock.Received().SendPasswordResetEmail(Arg.Any<string>(), Arg.Any<string>());
            Assert.AreEqual(null, response);
        }

        [Test]
        public async Task ResetPassword_rong_id_Test()
        {
            // Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await AuthServiceMock.ResetPassword("id", "idddddd"));

            // Assert
            Assert.AreEqual("Wrong id was given", exception.Message);
        }


        [Test]
        public async Task ResetPassword_success_Test()
        {
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(athlete);
            // Act
           await AuthServiceMock.ResetPassword("5e7b9bd0a04cca0001ffd0c2", "idddddd");

            // Assert
            await AuthRepositoryMock.Received().ChangePassword(Arg.Any<string>(), Arg.Any<HashPasswordInfo>());
            await EmailRepositoryMock.Received().SendNewPassword(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public async Task InsertNewAdmin_password_must_contain_letters_numbers_test()
        {
            newUser.Password = "passsss";

            // Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await AuthServiceMock.InsertNewAdmin(newUser));

            // Assert
            Assert.AreEqual("Password must contain atleast one number and atleast one letter", exception.Message);
        }
        [Test]
        public async Task InsertNewAdmin_email_Already_Exist_Test()
        {
            // Arrange
            AuthRepositoryMock.CheckIfEmailAlreadyExist(Arg.Any<ConsumerEntity>()).Returns("email already exist");

            // Act
            var response = await AuthServiceMock.InsertNewAdmin(newUser);

            // Assert
            Assert.AreEqual("email already exist", response);
        }

        [Test]
        public async Task InsertAdmin_success_Tests()
        {
            // Arrange
            AuthRepositoryMock.CheckIfEmailAlreadyExist(Arg.Any<ConsumerEntity>()).Returns((string)null);
            AuthRepositoryMock.RegisterUser(Arg.Any<ConsumerEntity>()).Returns(newUser);

            // Act
            var response = await AuthServiceMock.Register(newUser);

            // Assert
            Assert.AreEqual(null, response);
        }



        //Controllers



    }
}





