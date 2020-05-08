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

namespace TMSTesting
{
    public class PersonalTrainingControllerTests
    {       
               
        public IPersonalTrainingsRepository PersonalTrainingRepositoryMock;
        public IAggregateRepository AggregateRepoMock;
        public IPersonalTrainingService PersonalTrainingServiceMock;

        public PersonalTrainingController PersonalTrainingControllerMock;
        
        public List<PersonalTrainingEntity> personalTrainings;
        public PersonalTraining personalTrain;
        public PersonalTrainingEntity personalTrainingentity;
        public List<CoachAssignedTrains> coachTrains;
        public List<TrainingsWhichAreAssignedByDate> assignedTrainings;
        public ClaimsPrincipal user;
        [SetUp]
        public void Setup()
        {
            PersonalTrainingRepositoryMock = Substitute.For<IPersonalTrainingsRepository>();
            AggregateRepoMock = Substitute.For<IAggregateRepository>();
            PersonalTrainingServiceMock = Substitute.For<IPersonalTrainingService>();
            PersonalTrainingControllerMock = new PersonalTrainingController()
            {
              personalTrainingService= PersonalTrainingServiceMock,
              aggregateRepo= AggregateRepoMock,
              trainingRepo= PersonalTrainingRepositoryMock
            };

             user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
              {
                 new Claim(ClaimTypes.NameIdentifier, "testId"),
                 new Claim(ClaimTypes.Name, "testName"),
                 new Claim(ClaimTypes.Role, "Coach")
              }));

            personalTrain = new PersonalTraining()
            {
                AthleteIds = new List<string>() { "5e7b9bd0a04cca0001ffd0c2" },
                CoachId = "5e7b9c1f9805e2000126e82e",
                Day = DateTime.Now,
                Description = "nauja",
                Place = "inside",
                TrainTemplateId = "5e7b9c1f9885e2000126e82e"
            };

            personalTrainingentity = new PersonalTrainingEntity()
            {
                AthleteId = "5e7b9bd0a04cca0001ffd0c2",
                CoachId = "5e7b9c1f9805e2000126e82e",
                Day = DateTime.Now,
                Description = "nauja",
                Place = "inside",
                TrainTemplateId = "5e7b9c1f9885e2000126e82e",
                AthleteReport = "sunku",
                Id = "",
                Results = new List<SetEntity>(){new SetEntity()
                { Distance=400,
                Pace=50,
                Rest=30}
                }
            };
            personalTrainings = new List<PersonalTrainingEntity>() { personalTrainingentity };

            coachTrains = new List<CoachAssignedTrains>() { new CoachAssignedTrains() { Day = DateTime.Now, Description = "oho" } };
            assignedTrainings = new List<TrainingsWhichAreAssignedByDate>() { new TrainingsWhichAreAssignedByDate() { 
                Athlete = "aur", AthleteId = "ID",Definition="do",PersonalTrainingId="id",TrainingTemplateId="id" } };
        }
               

       

        [Test]
        public async Task PostTraining_success_Test()
        {

            PersonalTrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
   

            // Act


            var actionResult = await PersonalTrainingControllerMock.CreatePersonalTraining(personalTrain);
            // Assert
            await PersonalTrainingServiceMock.Received().ProcessPersonalTraining(Arg.Any<PersonalTraining>());
            Assert.IsInstanceOf<OkResult>(actionResult);
        }

        [Test]
        public async Task GetPersonalTrainingsByAthlete_success_Test()
        {

            PersonalTrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange

            PersonalTrainingRepositoryMock.GetAllPersonalTrainingsAthlete(Arg.Any<string>()).Returns(personalTrainings);
            // Act


            var actionResult = await PersonalTrainingControllerMock.GetPersonalTrainingsByAthlete();
            // Assert
            await PersonalTrainingRepositoryMock.Received().GetAllPersonalTrainingsAthlete(Arg.Any<string>());
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task GetPersonalTrainingsCount_success_Test()
        {

            PersonalTrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange

            AggregateRepoMock.GetCoachAssignedTrainingsCount(Arg.Any<string>()).Returns(coachTrains);
            // Act


            var actionResult = await PersonalTrainingControllerMock.GetPersonalTrainingsCount();
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task GetAssignedPersonalTrainings_success_Test()
        {

            PersonalTrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange

            AggregateRepoMock.TrainingsWhichAssignedByDate(Arg.Any<string>(), Arg.Any<string>()).Returns(assignedTrainings);
            // Act


            var actionResult = await PersonalTrainingControllerMock.GetAssignedPersonalTrainings("2020-12-05");
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task GetPersonalTrainingsWhichGiven_success_Test()
        {

            PersonalTrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange

            // Act


            var actionResult = await PersonalTrainingControllerMock.GetPersonalTrainingsWhichGiven();
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }


        [Test]
        public async Task GetPersonalTraining_success_Test()
        {

            PersonalTrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            PersonalTrainingRepositoryMock.GetPersonalTrainingByID(Arg.Any<string>()).Returns(personalTrainingentity);

            // Act
            var actionResult = await PersonalTrainingControllerMock.GetPersonalTraining("id");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task GetPersonalTrainingByDate_success_Test()
        {

            PersonalTrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            PersonalTrainingRepositoryMock.GetPersonalTrainingByDate(Arg.Any<string>(), Arg.Any<string>()).Returns(personalTrainingentity);

            // Act
            var actionResult = await PersonalTrainingControllerMock.GetPersonalTrainingByDate("id");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task DeleteTraining_unauthorized_Test()
        {

            PersonalTrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            PersonalTrainingServiceMock.DeletePersonalTrain(Arg.Any<string>(), Arg.Any<string>()).Returns((List<TrainingsWhichAreAssignedByDate>)null);

            // Act
            var actionResult = await PersonalTrainingControllerMock.DeleteTraining("id");

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(actionResult);
        }

        [Test]
        public async Task DeleteTraining_success_Test()
        {

            PersonalTrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            PersonalTrainingServiceMock.DeletePersonalTrain(Arg.Any<string>(), Arg.Any<string>()).Returns(assignedTrainings);

            // Act
            var actionResult = await PersonalTrainingControllerMock.DeleteTraining("id");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

         [Test]
        public async Task GetFreeAthletes_success_coun_0_Test()
        {

            PersonalTrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            AggregateRepoMock.GetFreeAthletesByDayAggregate(Arg.Any<string>(), Arg.Any<string>()).Returns(new List<PersonInfo>());
            PersonalTrainingServiceMock.GetAthletesIfFree(Arg.Any<string>(), Arg.Any<string>()).Returns(new List<PersonInfo>());

            // Act
            var actionResult = await PersonalTrainingControllerMock.GetFreeAthletes("2020-12-05");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);

        }

        [Test]
        public async Task GetFreeAthletes_success_coun_not0_Test()
        {

            PersonalTrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            AggregateRepoMock.GetFreeAthletesByDayAggregate(Arg.Any<string>(), Arg.Any<string>()).Returns(new List<PersonInfo>() { new PersonInfo { IdPerson="id"} });
            

            // Act
            var actionResult = await PersonalTrainingControllerMock.GetFreeAthletes("2020-12-05");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);

        }


    }
}





