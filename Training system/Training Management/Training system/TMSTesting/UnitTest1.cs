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
        public async Task SendEmail()
        {
           await emailRepo.SendEmailConfirmationEmail("valauskas.aurimas@gmail.com", "5e972ad851ae1a0001dde044");
         
        }

        [Test]
        public async Task SendEmailTry()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("aur.val10@ktu.lt", "aurimas");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("valauskas.aurimas@gmail.com", "kitam");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

        }
    }
}