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
using TMS.Contracts.Repositories;
using TMS.Contracts.Repositories.InternalManagement;
using TMS.Contracts.Services.InternalManagement;
using TMS.Controllers.TrainingManagement;

namespace TMSTesting
{
    public class PersonalManagementControllerTests
    {   
               
        public IPersonalManagementService PersonalManagementServiceMock;
        public IConsumerRepository ConsumerRepositoryMock;
        public IAuthService AuthServiceMock;
        public IAggregateRepository AggregateRepositoryMock;

        public PersonalManagementController PersonalManagementControllerMock;
        public ClaimsPrincipal user;
        public List<PersonalRecords> personalRecords;
        public List<PersonInfo> persons;
        public List<CompetitionsAggregate> personalCompetitions;
        public CompetitionEntity competition;
        public ConsumerEntity consumer;


        [SetUp]
        public void Setup()
        {
            PersonalManagementServiceMock = Substitute.For<IPersonalManagementService>();
            ConsumerRepositoryMock = Substitute.For<IConsumerRepository>();
            AuthServiceMock = Substitute.For<IAuthService>();
            AggregateRepositoryMock = Substitute.For<IAggregateRepository>();
            PersonalManagementControllerMock = new PersonalManagementController()
            {
              personalManagementService= PersonalManagementServiceMock,
              aggregateRepo= AggregateRepositoryMock,
              authService= AuthServiceMock,
              ConsumerRepository= ConsumerRepositoryMock
            };

             user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
              {
                 new Claim(ClaimTypes.NameIdentifier, "testId"),
                 new Claim(ClaimTypes.Name, "testName"),
                 new Claim(ClaimTypes.Role, "Athlete")
              }));
            personalRecords = new List<PersonalRecords>() { new PersonalRecords() { Records = new List<Record>() { new Record() { Competition = "a" } } } };
            personalCompetitions = new List<CompetitionsAggregate>() { new CompetitionsAggregate() { Records = new List<Record>() { new Record() { Competition = "a" } } } };
            competition = new CompetitionEntity(){ CompetitionName = "Lt" };

            consumer = new ConsumerEntity()
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
            persons = new List<PersonInfo>() { new PersonInfo() { IdPerson = "id" } };
        }
               

       

        [Test]
        public async Task GetRecords_success_Test()
        {

            PersonalManagementControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            AggregateRepositoryMock.RecordAggregate(Arg.Any<string>()).Returns(personalRecords);
            // Act
            var actionResult = await PersonalManagementControllerMock.GetRecords();
            // Assert  
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task GetOtherRecords_success_Test()
        {

            PersonalManagementControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange

            AggregateRepositoryMock.RecordAggregate(Arg.Any<string>()).Returns(personalRecords);
            // Act

            var actionResult = await PersonalManagementControllerMock.GetOtherRecords("id");
            // Assert

            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task GetCompetitions_success_Test()
        {

            PersonalManagementControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange

            AggregateRepositoryMock.CompetitionsAggregate(Arg.Any<string>()).Returns(personalCompetitions);
            // Act

            var actionResult = await PersonalManagementControllerMock.GetCompetitions();
            // Assert

            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task GetOtherCompetition_success_Test()
        {

            PersonalManagementControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange

            AggregateRepositoryMock.CompetitionsAggregate(Arg.Any<string>()).Returns(personalCompetitions);
            // Act

            var actionResult = await PersonalManagementControllerMock.GetOtherCompetition("id");
            // Assert

            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task AddCompetitionTime_success_Test()
        {

            PersonalManagementControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange


            // Act
            var actionResult = await PersonalManagementControllerMock.AddCompetitionTime(competition);

            // Assert
            await PersonalManagementServiceMock.Received().AddCompetitionToListOrSetNewRecord(Arg.Any<string>(), Arg.Any<CompetitionEntity>());
            Assert.IsInstanceOf<OkResult>(actionResult);
        }


        [Test]
        public async Task SendInvite_BadRequest_Test()
        {

            PersonalManagementControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange

            PersonalManagementServiceMock.SendInviteToAnother(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns("error");
            // Act

            var actionResult = await PersonalManagementControllerMock.SendInvite(consumer);
            // Assert

            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }
        [Test]
        public async Task SendInvite_success_Test()
        {

            PersonalManagementControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange

            PersonalManagementServiceMock.SendInviteToAnother(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns((string)null);
            // Act

            var actionResult = await PersonalManagementControllerMock.SendInvite(consumer);
            // Assert

            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task AcceptInvitation_success_Test()
        {

            PersonalManagementControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange

            AggregateRepositoryMock.GetAllInvitersAggregate(Arg.Any<string>()).Returns(persons);
            // Act

            var actionResult = await PersonalManagementControllerMock.AcceptInvitation(consumer);
            // Assert

            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task DeclineInvitation_success_Test()
        {

            PersonalManagementControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange

            AggregateRepositoryMock.GetAllInvitersAggregate(Arg.Any<string>()).Returns(persons);
            // Act

            var actionResult = await PersonalManagementControllerMock.DeclineInvitation(consumer);
            // Assert

            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task GetInvitations_success_Test()
        {

            PersonalManagementControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange

            AggregateRepositoryMock.GetAllInvitersAggregate(Arg.Any<string>()).Returns(persons);
            // Act

            var actionResult = await PersonalManagementControllerMock.GetInvitations();
            // Assert

            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task GetPersonalInfo_success_Test()
        {

            PersonalManagementControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange

            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(consumer);
            // Act

            var actionResult = await PersonalManagementControllerMock.GetPersonalInfo();
            // Assert

            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task ChangePassword_success_Test()
        {

            PersonalManagementControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange

            // Act
            var actionResult = await PersonalManagementControllerMock.ChangePassword(consumer);
            // Assert
            await AuthServiceMock.Received().ChangePassword(Arg.Any<string>(), Arg.Any<string>());
            Assert.IsInstanceOf<OkResult>(actionResult);
        }


        [Test]
        public async Task GetAthleteCoach_success_Test()
        {

            PersonalManagementControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            AggregateRepositoryMock.AthletePersonalCoachAggregate(Arg.Any<string>()).Returns(persons);
            // Act
            var actionResult = await PersonalManagementControllerMock.GetAthleteCoach();
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task GetConsumerFriends_success_Test()
        {

            PersonalManagementControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            AggregateRepositoryMock.GetUserFriendsAggregate(Arg.Any<string>()).Returns(persons);
            // Act
            var actionResult = await PersonalManagementControllerMock.GetConsumerFriends();
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }


        [Test]
        public async Task GetAthletes_success_Test()
        {

            PersonalManagementControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            AggregateRepositoryMock.GetAllCoachAthletesAggregate(Arg.Any<string>()).Returns(persons);
            // Act
            var actionResult = await PersonalManagementControllerMock.GetAthletes();
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }
    }
}





