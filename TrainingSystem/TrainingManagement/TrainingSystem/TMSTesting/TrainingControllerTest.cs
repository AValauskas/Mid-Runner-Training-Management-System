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
    public class TrainingControllerTests
    {       
               
        public ITrainingsReposiry TrainingRepoMock;
        public ITrainingService TrainingServcieMock;
        public TrainingController TrainingControllerMock;
        public TrainingEntity training;
        public List<TrainingEntity> trainings;

        public ClaimsPrincipal user;
        [SetUp]
        public void Setup()
        {
            TrainingRepoMock = Substitute.For<ITrainingsReposiry>();
            TrainingServcieMock = Substitute.For<ITrainingService>();
            TrainingControllerMock = new TrainingController()
            {
                trainingRepo = TrainingRepoMock,
                trainingService= TrainingServcieMock
            };

             user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
              {
                 new Claim(ClaimTypes.NameIdentifier, "testId"),
                 new Claim(ClaimTypes.Name, "testName"),
                 new Claim(ClaimTypes.Role, "Coach")
              }));

            training = new TrainingEntity()
            {
                Description = "fun",
                Destinition = 800,
                Id = "chaa",
                IsPersonal = false,
                Level = 5,
                Owner = "aurims",
                Repeats = 3,
                SeasonTime = "5e7b9c1f9885e2000126e82e",
                Sets = new List<SetEntity>() { new SetEntity() { Distance = 300, Pace = 20, Rest = 30 } },
                ToDisplay = "2x2",
                TrainingType = "as id fdghtrhgetgregfsadgffsdgsfdgtyutyj name sdfsadagfsdgdsfgfsd5e7b9c1f9885e2000126e82e taxonomy fdgsdhrthrteyurhdfghfdgasdgfrewgterg  ",
                TrainingTypeName = "5e7b9c1f9885e2000126e82e"
            };

            trainings = new List<TrainingEntity>() { training };

        }
               

       

        [Test]
        public async Task PostTraining_success_Test()
        {

            TrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            TrainingServcieMock.InsertTraining(Arg.Any<TrainingEntity>(), Arg.Any<string>()).Returns(trainings);

            // Act

            var actionResult = await TrainingControllerMock.PostTraining(training);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }


        [Test]
        public async Task GetAllAvailableTrainings_success_Test()
        {

            TrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            TrainingRepoMock.GetAllAvailableTrainings(Arg.Any<string>()).Returns(trainings);

            // Act

            var actionResult = await TrainingControllerMock.GetAllAvailableTrainings();
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }


      
        [Test]
        public async Task GetAllTrainings_success_Test()
        {

            TrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            TrainingRepoMock.GetAllTrainings().Returns(trainings);

            // Act

            var actionResult = await TrainingControllerMock.GetAllTrainings();
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }


        [Test]
        public async Task GetTrainingsByTaxonomy_success_Test()
        {

            TrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            TrainingRepoMock.GetTrainingsByType(Arg.Any<string>(), Arg.Any<string>()).Returns(trainings);

            // Act

            var actionResult = await TrainingControllerMock.GetTrainingsByTaxonomy("id");
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

     
        [Test]
        public async Task GetTraining_success_Test()
        {

            TrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            TrainingRepoMock.GetTrainingByID(Arg.Any<string>()).Returns(training);

            // Act

            var actionResult = await TrainingControllerMock.GetTraining("id");
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task DeleteTraining_unauthorized_Test()
        {

            TrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            TrainingServcieMock.CheckIfTrainingBelongToRightPerson(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            // Act

            var actionResult = await TrainingControllerMock.DeleteTraining("id");
            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(actionResult);
        }

        [Test]
        public async Task DeleteTraining_success_Test()
        {

            TrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            TrainingServcieMock.CheckIfTrainingBelongToRightPerson(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
            TrainingServcieMock.DeleteTraining(Arg.Any<string>(), Arg.Any<string>()).Returns(trainings);
            // Act

            var actionResult = await TrainingControllerMock.DeleteTraining("id");
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task UpdateTraining_unauthorized_Test()
        {

            TrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            TrainingServcieMock.CheckIfTrainingBelongToRightPerson(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            // Act

            var actionResult = await TrainingControllerMock.UpdateTraining(training);
            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(actionResult);
        }

        [Test]
        public async Task UpdateTraining_success_Test()
        {

            TrainingControllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            // Arrange
            TrainingServcieMock.CheckIfTrainingBelongToRightPerson(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
            TrainingServcieMock.DeleteTraining(Arg.Any<string>(), Arg.Any<string>()).Returns(trainings);
            // Act

            var actionResult = await TrainingControllerMock.UpdateTraining(training);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }
    }
}





