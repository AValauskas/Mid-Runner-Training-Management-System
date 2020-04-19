using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using TMS;

namespace TMSTesting
{
    public class Tests
    {

        IEmailRepository emailRepo;
        [SetUp]
        public void Setup()
        {
            emailRepo = new EmailRepository();



        }

        [Test]
        public void SendEmail()
        {
            emailRepo.SendEmailConfirmationEmail("valauskas.aurimas@gmail.com", "5e7b9bd0a04cca0001ffd0c2");
         
        }

        [Test]
        public async Task SendEmailTry()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("aurimas19970704@gmail.com", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("valauskas.aurimas@gmail.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

        }
    }
}