using NSubstitute;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMS;
using TMS.Contracts.Repositories;
using TMS.Contracts.Repositories.TrainingManagement;
using TMS.Contracts.Services.TrainingManagement;
using TMS.Repositories;
using TMS.Repositories.TrainingManagement;
using TMS.Services.TrainingManagement;

namespace TMSTesting
{
    public class TrainTests
    {


        public IPersonalTrainingsRepository PersonalTrainingsRepository;
        public IAggregateRepository AggregateRepository;
        public IPersonalTrainingService PersonalTrainingService;

        public IPersonalTrainingsRepository PersonalTrainingsRepositoryMock;
        public IAggregateRepository AggregateRepositoryMock;
        public IPersonalTrainingService PersonalTrainingMock;
        public PersonalTraining personalTrain;
        public PersonalTrainingEntity personalTrainingentity;
        public List<TrainingsWhichAreAssignedByDate> trainsAssigned;
        public ConsumerEntity athlete;
        public ConsumerEntity coach;

        [SetUp]
        public void Setup()
        {
            
            PersonalTrainingsRepository = new PersonalTrainingRepository();
            AggregateRepository = new AggregateRepository();
            PersonalTrainingService = new PersonalTrainingService()
            {
                AggregateRepository = AggregateRepository,
                PersonalTrainingRepo = PersonalTrainingsRepository
            };

            PersonalTrainingsRepositoryMock = Substitute.For<IPersonalTrainingsRepository>();
            AggregateRepositoryMock = Substitute.For<IAggregateRepository>();
            PersonalTrainingMock = new PersonalTrainingService()
            {
                
                AggregateRepository = AggregateRepositoryMock,
                PersonalTrainingRepo = PersonalTrainingsRepositoryMock
            };

            personalTrain = new PersonalTraining()
            {
                AthleteIds = new List<string>() { "5e7b9bd0a04cca0001ffd0c2" },
                CoachId = "5e7b9c1f9805e2000126e82e",
                Day = DateTime.Now,
                Description = "nauja",
                Place = "inside",
                TrainTemplateId= "5e7b9c1f9885e2000126e82e"
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
                Results =new List<SetEntity>(){new SetEntity()
                { Distance=400,
                Pace=50,
                Rest=30}
                }
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
                Id = "5e7b9bd0a04cca0001ffd0c2",
                InviteFrom = new List<string>(),
                Password = "izgZDGqCYiqLTdNrWX19KjkzGjuRdBPVmPKbZQv9XVI=",
                Records = new List<CompetitionEntity> { },
                Role = "Athlete",
                Salt = "W3EuL37eopFmwMB+q/rHQg=="
            };
            coach = new ConsumerEntity()
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
                Password = "ScP2mmH8K6cmwSkF28RRblQfR5uoNAG4FfAoTs8xlAk==",
                Records = new List<CompetitionEntity> { },
                Role = "Coach",
                Salt = "0iiWScc3aka3f9MjTm21cQ=="
            };

            trainsAssigned =new List<TrainingsWhichAreAssignedByDate>(){ new TrainingsWhichAreAssignedByDate()
            {
                Athlete = "Aurimas",
                AthleteId = "5e7b9bd0a04cca0001ffd0c2",
                Definition = "geras",
                PersonalTrainingId = "aha",
                TrainingTemplateId = "d"
            }};
        }

        [Test]
        public async Task ProcessPersonalTraining_Test()
        {
                      
            // Act
             await PersonalTrainingMock.ProcessPersonalTraining(personalTrain);

            // Assert            
            await PersonalTrainingsRepositoryMock.Received().InsertManyPersonalTrainings(Arg.Any<List<PersonalTrainingEntity>>());
          
        }

        [Test]
        public async Task DeletePersonalTrain_coach_not_owner_Test()
        {
            PersonalTrainingsRepositoryMock.GetPersonalTrainingByID(Arg.Any<string>()).Returns(personalTrainingentity);
            // Act
           var response = await PersonalTrainingMock.DeletePersonalTrain("asfdsf", "5e7b9c1f9885e2000126e82e");

            // Assert            
            Assert.AreEqual(null, response);

        }
        [Test]
        public async Task DeletePersonalTrain_success_Test()
        {
            PersonalTrainingsRepositoryMock.GetPersonalTrainingByID(Arg.Any<string>()).Returns(personalTrainingentity);
            AggregateRepositoryMock.TrainingsWhichAssignedByDate(Arg.Any<string>(), Arg.Any<string>()).Returns(trainsAssigned);
            // Act
            var response = await PersonalTrainingMock.DeletePersonalTrain("5e7b9c1f9805e2000126e82e", "5e7b9c1f9885e2000126e82e");


            await PersonalTrainingsRepositoryMock.Received().DeleteTraining(Arg.Any<string>());
            Assert.AreEqual(trainsAssigned, response);

        }

        [Test]
        public async Task CheckIfPersonCanUpdatePersonalTrainingReport_fail_Test()
        {
            PersonalTrainingsRepositoryMock.GetPersonalTrainingByID(Arg.Any<string>()).Returns(personalTrainingentity);
            // Act
            var response = await PersonalTrainingMock.CheckIfCanUpdate("5e7b9c1f980hfgh5e2000126e82e", "5e7b9c1f9885e20001dgh26e82e");
                       
            Assert.AreEqual(false, response);
        }

        [Test]
        public async Task CheckIfPersonCanUpdatePersonalTrainingReport_success_Test()
        {
            PersonalTrainingsRepositoryMock.GetPersonalTrainingByID(Arg.Any<string>()).Returns(personalTrainingentity);
            // Act
            var response = await PersonalTrainingMock.CheckIfCanUpdate("5e7b9c1f9805e2000126e82e", "5e7b9c1f9885e2000126e82e");

            Assert.AreEqual(true, response);
        }

        [Test]
        public async Task GetAthletesIfFree_empty_Test()
        {
            PersonalTrainingsRepositoryMock.CheckIfCoachHasTrainingInChoosenDay(Arg.Any<DateTime>(), Arg.Any<string>()).Returns(true);
            // Act
            var response = await PersonalTrainingMock.GetAthletesIfFree("5e7b9c1f9805e2000126e82e", "2020-10-12");

            Assert.AreEqual(response.Count, 0);
        }
        [Test]
        public async Task GetAthletesIfFree_success_Test()
        {

            var coachAggregate = new List<PersonInfo> { new PersonInfo() { Name = "nm", IdPerson = "id", Surname = "sur" } };
            PersonalTrainingsRepositoryMock.CheckIfCoachHasTrainingInChoosenDay(Arg.Any<DateTime>(), Arg.Any<string>()).Returns(false);
            AggregateRepositoryMock.GetAllCoachAthletesAggregate( Arg.Any<string>()).Returns(coachAggregate);
            // Act
            var response = await PersonalTrainingMock.GetAthletesIfFree("5e7b9c1f9805e2000126e82e", "2020-10-12");

            Assert.AreNotEqual(response.Count, 0);
        }

    }
}