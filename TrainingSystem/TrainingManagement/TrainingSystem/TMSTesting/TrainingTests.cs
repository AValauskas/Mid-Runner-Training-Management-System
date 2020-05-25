using NSubstitute;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMS;
using TMS.Contracts.Repositories.TrainingManagement;
using TMS.Contracts.Services.TrainingManagement;
using TMS.Repositories.TrainingManagement;
using TMS.Services.TrainingManagement;

namespace TMSTesting
{
    public class TrainingTests
    {
        public ITrainingsRepository TrainingRepository;
        public ITrainingService TrainingService;

        public ITrainingsRepository TrainingRepositoryMock;
        public ITrainingService TrainingServiceMock;
        public TrainingEntity training;
        public List<TrainingEntity> trainings;
        [SetUp]
        public void Setup()
        {
            TrainingRepository = new TrainingsRepository();
            TrainingService = new TrainingService()
            {
                TrainingRepo = TrainingRepository
            };

            TrainingRepositoryMock = Substitute.For<ITrainingsRepository>();
            TrainingServiceMock = new TrainingService()
            {
                TrainingRepo = TrainingRepositoryMock
            };
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
        public async Task CheckIfTrainingBelongToRightPerson_false_Test()
        {
            training.Owner = "wrong";
            TrainingRepositoryMock.GetTrainingByID(Arg.Any<string>()).Returns(training);
            // Act
            var response = await TrainingServiceMock.CheckIfTrainingBelongToRightPerson("asfdsf", "5e7b9c1f9885e2000126e82e");

            // Assert            
            Assert.AreEqual(false, response);
        }

        [Test]
        public async Task CheckIfTrainingBelongToRightPerson_true_Test()
        {
            training.Owner = "correct";
            TrainingRepositoryMock.GetTrainingByID(Arg.Any<string>()).Returns(training);
            // Act
            var response = await TrainingServiceMock.CheckIfTrainingBelongToRightPerson("correct", "5e7b9c1f9885e2000126e82e");

            // Assert            
            Assert.AreEqual(true, response);
        }

        [Test]
        public async Task InsertTraining_success_Test()
        {
            TrainingRepositoryMock.GetAllAvailableTrainings(Arg.Any<string>()).Returns(trainings);
            // Act
            var response = await TrainingServiceMock.InsertTraining(training, "5e7b9c1f9885e2000126e82e");

            // Assert            
            await TrainingRepositoryMock.Received().InsertTraining(Arg.Any<TrainingEntity>());
            Assert.AreNotEqual(0, response.Count);
        }

        [Test]
        public async Task UpdateTraining_success_Test()
        {
            TrainingRepositoryMock.GetAllAvailableTrainings(Arg.Any<string>()).Returns(trainings);
            // Act
            var response = await TrainingServiceMock.UpdateTraining(training, "5e7b9c1f9885e2000126e82e");

            // Assert            
            await TrainingRepositoryMock.Received().ReplaceTraining(Arg.Any<TrainingEntity>());
            Assert.AreNotEqual(0, response.Count);
        }

        [Test]
        public async Task DeleteTraining_success_Test()
        {
            TrainingRepositoryMock.GetAllAvailableTrainings(Arg.Any<string>()).Returns(trainings);
            // Act
            var response = await TrainingServiceMock.DeleteTraining("train", "5e7b9c1f9885e2000126e82e");

            // Assert            
            await TrainingRepositoryMock.Received().DeleteTraining(Arg.Any<string>());
            Assert.AreNotEqual(0, response.Count);
        }
    }
}