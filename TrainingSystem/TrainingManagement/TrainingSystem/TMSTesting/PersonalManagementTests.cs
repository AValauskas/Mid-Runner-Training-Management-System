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
    public class PersonalManagementTests
    {
        public IConsumerRepository ConsumerRepository;
        public IPersonalTrainingsRepository PersonalTrainingsRepository;
        public IAggregateRepository AggregateRepository;
        public IPersonalManagementService PersonalManagementService;
     
          
        public IConsumerRepository ConsumerRepositoryMock;
        public IPersonalTrainingsRepository PersonalTrainingsRepositoryMock;
        public IAggregateRepository AggregateRepositoryMock;
        public IPersonalManagementService PersonalManagementServiceMock;
        public CompetitionEntity competition;
        public ConsumerEntity athlete;
        public ConsumerEntity coach;

        [SetUp]
        public void Setup()
        {
            ConsumerRepository = new ConsumerRepository();
            PersonalTrainingsRepository = new PersonalTrainingRepository();
            AggregateRepository= new AggregateRepository();
            PersonalManagementService = new PersonalManagementService()
            {
                AggregateRepository= AggregateRepository,
                ConsumerRepository= ConsumerRepository,
                PersonalTrainingsRepository= PersonalTrainingsRepository
            };

            ConsumerRepositoryMock = Substitute.For<IConsumerRepository>();
            PersonalTrainingsRepositoryMock = Substitute.For<IPersonalTrainingsRepository>();
            AggregateRepositoryMock = Substitute.For<IAggregateRepository>();
            PersonalManagementServiceMock = new PersonalManagementService()
            {
                AggregateRepository = AggregateRepositoryMock,
                ConsumerRepository = ConsumerRepositoryMock,
                PersonalTrainingsRepository = PersonalTrainingsRepositoryMock
            };

            competition = new CompetitionEntity()
            {
                CompetitionName = "LT",
                Date = DateTime.Now,
                Distance = 400,
                Place = "inside",
                Time = 55.55
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
        }




        [Test]
        public async Task AddCompetitionToListOrSetNewRecord_Service_Record_not_Exist__Test()
        {
            // Arrange
           ConsumerRepositoryMock.CheckIfRecordExist(Arg.Any<string>(), Arg.Any<CompetitionEntity>()).Returns((ConsumerEntity)null);

            // Act
            await PersonalManagementServiceMock.AddCompetitionToListOrSetNewRecord("id", competition);

            // Assert
            await ConsumerRepositoryMock.Received().AddNewCompetition(Arg.Any<string>(), Arg.Any<CompetitionEntity>());
            await ConsumerRepositoryMock.Received().AddNewPersonalBest(Arg.Any<string>(), Arg.Any<CompetitionEntity>());
        }


        [Test]
        public async Task AddCompetitionToListOrSetNewRecord_Service_Record_lower_Test()
        {
            // Arrange
            ConsumerRepositoryMock.CheckIfRecordExist(Arg.Any<string>(), Arg.Any<CompetitionEntity>()).Returns(athlete);
            ConsumerRepositoryMock.CheckIfBiggerPersonalTimeExist(Arg.Any<string>(), Arg.Any<CompetitionEntity>()).Returns((ConsumerEntity)null);
            // Act
            await PersonalManagementServiceMock.AddCompetitionToListOrSetNewRecord("id", competition);

            // Assert
            await ConsumerRepositoryMock.Received().AddNewCompetition(Arg.Any<string>(), Arg.Any<CompetitionEntity>());
            await ConsumerRepositoryMock.Received().UpdatePersonalRecord(Arg.Any<string>(), Arg.Any<CompetitionEntity>());
        }


        [Test]
        public async Task AcceptInvitation_receiver_Athlete_sender_Coach_Test()
        {
            // Arrange
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(coach);
            
            // Act
            await PersonalManagementServiceMock.AcceptInvitation("id","Athlete","receiverId");

            // Assert
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            await ConsumerRepositoryMock.Received().AceptInvitationCoach(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public async Task AcceptInvitation_receiver_Athlete_sender_Athlete_Test()
        {
            // Arrange
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(athlete);

            // Act
            await PersonalManagementServiceMock.AcceptInvitation("id", "Athlete", "receiverId");

            // Assert
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            await ConsumerRepositoryMock.Received().AceptInvitationAthlete(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public async Task AcceptInvitation_receiver_Coach_sender_Athlete_Test()
        {
            // Arrange
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(athlete);

            // Act
            await PersonalManagementServiceMock.AcceptInvitation("id", "Coach", "receiverId");

            // Assert
            await ConsumerRepositoryMock.Received().AceptInvitationCoach(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public async Task SendInviteToAnother_Same_Person_Test()
        {

            var response = await PersonalManagementServiceMock.SendInviteToAnother("senderId", "Coach", "senderId");

            Assert.AreEqual(response, "you can't invite yourself");
        }



        [Test]
        public async Task SendInviteToAnother_wrong_code_Test()
        {
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("senderId", "Coach", "receiverId");

            // Assert
            Assert.AreEqual(response, "wrong code was written");
        }



        [Test]
        public async Task SendInviteToAnother_sender_athlete_receiver_notExist_Test()
        {
            // Arrange
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns((ConsumerEntity)null);
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("senderId", "Athlete", "5e7b9bd0a04cca0001ffd0c2");

            // Assert
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            Assert.AreEqual(response, "this person not exist");
        }
                              
        [Test]
        public async Task SendInviteToAnother_sender_athlete_receiver_already_has_invite_Test()
        {
            // Arrange
            coach.InviteFrom.Add("5e7b9bd0a04cca0001ffd0c2");
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(coach);
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("5e7b9bd0a04cca0001ffd0c2", "Athlete", "5e7b9c1f9805e2000126e82e");

            // Assert
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            Assert.AreEqual(response, "You already sent invitation");
        }

        [Test]
        public async Task SendInviteToAnother_sender_athlete_receiver_coach_already_belong_Test()
        {
            // Arrange
            coach.Athletes.Add("5e7b9bd0a04cca0001ffd0c2");
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(coach);
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("5e7b9bd0a04cca0001ffd0c2", "Athlete", "5e7b9c1f9805e2000126e82e");

            // Assert
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            Assert.AreEqual(response, "You already belong to this trainer");
        }

        [Test]
        public async Task SendInviteToAnother_sender_athlete_already_friends_belong_Test()
        {
            // Arrange
            coach.Friends.Add("5e7b9bd0a04cca0001ffd0c2");
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(coach);
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("5e7b9bd0a04cca0001ffd0c2", "Athlete", "5e7b9c1f9805e2000126e82e");

            // Assert
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            Assert.AreEqual(response, "You are already friends with this athlete");
        }
        [Test]
        public async Task SendInviteToAnother_sender_athlete_receiver_admin_Test()
        {
            // Arrange
            coach.Role = "Admin";
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(coach);
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("5e7b9bd0a04cca0001ffd0c2", "Athlete", "5e7b9c1f9805e2000126e82e");

            // Assert
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            Assert.AreEqual(response, "You can't invite admin");
        }

        [Test]
        public async Task SendInviteToAnother_sender_athlete_already_has_request_Test()
        {

            athlete.InviteFrom.Add("5e7b9c1f9805e2000126e82e");

            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(coach,athlete);
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("5e7b9bd0a04cca0001ffd0c2", "Athlete", "5e7b9c1f9805e2000126e82e");

            // Assert
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            Assert.AreEqual(response, "You already have request from this user");
        }


        [Test]
        public async Task SendInviteToAnother_sender_athlete_already_has_coach_Test()
        {
            var coachAggregate = new List<PersonInfo> { new PersonInfo() };
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(coach, athlete);
            AggregateRepositoryMock.AthletePersonalCoachAggregate(Arg.Any<string>()).Returns(coachAggregate);
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("5e7b9bd0a04cca0001ffd0c2", "Athlete", "5e7b9c1f9805e2000126e82e");

            // Assert
            await AggregateRepositoryMock.Received().AthletePersonalCoachAggregate(Arg.Any<string>());
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            Assert.AreEqual(response, "You already have coach");
        }


        [Test]
        public async Task SendInviteToAnother_sender_coach_already_train_athlete_Test()
        {
            coach.Athletes.Add("5e7b9bd0a04cca0001ffd0c2");
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(coach);
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("5e7b9c1f9805e2000126e82e", "Coach", "5e7b9bd0a04cca0001ffd0c2");

            // Assert
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            Assert.AreEqual(response, "You already train this athlete");
        }

        [Test]
        public async Task SendInviteToAnother_sender_coach_already_got_invitation_Test()
        {
            coach.InviteFrom.Add("5e7b9bd0a04cca0001ffd0c2");
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(coach);
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("5e7b9c1f9805e2000126e82e", "Coach", "5e7b9bd0a04cca0001ffd0c2");

            // Assert
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            Assert.AreEqual(response, "Athlete already sent you invitation");
        }
        [Test]
        public async Task SendInviteToAnother_sender_coach_receiver_not_exist_Test()
        {
            
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(coach,(ConsumerEntity)null);
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("5e7b9c1f9805e2000126e82e", "Coach", "5e7b9bd0a04cca0001ffd0c2");

            // Assert
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            Assert.AreEqual(response, "this person not exist");
        }

        [Test]
        public async Task SendInviteToAnother_sender_coach_already_sent_invite_exist_Test()
        {
            athlete.InviteFrom.Add("5e7b9c1f9805e2000126e82e");
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(coach, athlete);
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("5e7b9c1f9805e2000126e82e", "Coach", "5e7b9bd0a04cca0001ffd0c2");

            // Assert
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            Assert.AreEqual(response, "You have already sent invitation");
        }

        [Test]
        public async Task SendInviteToAnother_sender_coach_receiver_coach_Test()
        {
            athlete.Role = "Coach";
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(coach, athlete);
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("5e7b9c1f9805e2000126e82e", "Coach", "5e7b9bd0a04cca0001ffd0c2");

            // Assert
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            Assert.AreEqual(response, "You can invite only athletes");
        }

        [Test]
        public async Task SendInviteToAnother_sender_coach_receiver_admin_Test()
        {
            athlete.Role = "Admin";
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(coach, athlete);
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("5e7b9c1f9805e2000126e82e", "Coach", "5e7b9bd0a04cca0001ffd0c2");

            // Assert
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            Assert.AreEqual(response, "You can't invite admin");
        }

        [Test]
        public async Task SendInviteToAnother_sender_coach_receiver_has_coach_Test()
        {

            var coachAggregate = new List<PersonInfo> { new PersonInfo() { Name="nm", IdPerson="id",Surname="sur"} };
            AggregateRepositoryMock.AthletePersonalCoachAggregate(Arg.Any<string>()).Returns(coachAggregate);

            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(coach, athlete);
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("5e7b9c1f9805e2000126e82e", "Coach", "5e7b9bd0a04cca0001ffd0c2");

            // Assert
            await AggregateRepositoryMock.Received().AthletePersonalCoachAggregate(Arg.Any<string>());
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            Assert.AreEqual(response, "This athlete already have coach");
        }


        [Test]
        public async Task SendInviteToAnother_sender_athlete_succesfully_sent()
        {
            var coachAggregate = new List<PersonInfo> { };
            ConsumerRepositoryMock.FindConsumerById(Arg.Any<string>()).Returns(coach, athlete);
            AggregateRepositoryMock.AthletePersonalCoachAggregate(Arg.Any<string>()).Returns(coachAggregate);
            // Act
            var response = await PersonalManagementServiceMock.SendInviteToAnother("5e7b9bd0a04cca0001ffd0c2", "Athlete", "5e7b9c1f9805e2000126e82e");

            // Assert

            await AggregateRepositoryMock.Received().AthletePersonalCoachAggregate(Arg.Any<string>());
            await ConsumerRepositoryMock.Received().FindConsumerById(Arg.Any<string>());
            await ConsumerRepositoryMock.Received().SendInviteToAnother(Arg.Any<string>(), Arg.Any<string>());            
        }
    }
}



