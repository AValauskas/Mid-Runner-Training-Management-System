using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class EmailRepository:IEmailRepository
    {
        private readonly SendGridClient Client = new SendGridClient(Settings.SendGridApiKey);

        public async Task SendEmailConfirmationEmail(string email, string token)
        {
            var message = new SendGridMessage();
            message.SetFrom("aurimas.valauskas@ktu.edu", "aurimas");
            message.AddTo("valauskas.aurimas@gmail.com");
            message.SetTemplateId("d - b702ab0729b242398c66eebf6c4d7758");
            message.SetTemplateData(new { token });

            _ = await Client.SendEmailAsync(message);
        }






        public async Task SendPasswordResetEmail(string email, string token)
        {
            var message = new SendGridMessage();
            message.SetFrom(Settings.SenderEmailAddress, Settings.SenderEmailAddressName);
            message.AddTo(email);
            message.SetTemplateId(Settings.PasswordResetEmailTemplateId);
            message.SetTemplateData(new { token });

            _ = await Client.SendEmailAsync(message);
        }
    }
}
