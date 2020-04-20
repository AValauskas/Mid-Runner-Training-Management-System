using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Environment.SetEnvironmentVariable("SENDGRID_API_KEY", "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");
            Environment.SetEnvironmentVariable("SENDGRID_API_KEY", "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");
            Execute().Wait();
        }
        static async Task Execute()
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY"); 
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("aurimas.valauskas@ktu.edu", "Aurimas");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("valauskas.aurimas@gmail.com", "ar gauna");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
