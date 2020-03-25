using NUnit.Framework;
using System.Threading.Tasks;
using Training_Management;

namespace TrainingSystemTesting
{
    public class Tests
    {
        IAuthService authService;
        IAuthRepository authRepo;
        [SetUp]
        public void Setup()
        {
            authRepo = new AuthRepository();
            authService = new AuthService()
            {
                AthleteRepository = new AthleteRepository(),
                CoachRepository = new CoachRepository()
            };
        }

        [Test]
        public async Task Login_Test()
        {
            await authRepo.LoginUser("valius@gmail.com", "valius");
        }

        [Test]
        public void Registration_Tests()
        {
            authRepo.RegisterUser("valius@gmail.com", "valius", "athlete");
        }
    }
}