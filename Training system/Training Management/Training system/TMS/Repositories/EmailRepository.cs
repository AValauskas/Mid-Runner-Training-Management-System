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
            message.SetFrom(Settings.SenderEmailAddress, Settings.SenderEmailAddressName);
            message.AddTo(email);
            message.SetTemplateId(Settings.EmailConfirmationTemplateId);
            message.SetTemplateData(new { token });
            
            var response = await Client.SendEmailAsync(message);            
        }                             

        public async Task SendPasswordResetEmail(string email, string token)
        {
            var message = new SendGridMessage();
            message.SetFrom(Settings.SenderEmailAddress, Settings.SenderEmailAddressName);
            message.AddTo(email);
            message.SetTemplateId(Settings.PasswordResetEmailTemplateId);
            message.SetTemplateData(new { token });

            var response = await Client.SendEmailAsync(message);
        }

        public async Task SendNewPassword(string email, string token)
        {
            var message = new SendGridMessage();
            message.SetFrom(Settings.SenderEmailAddress, Settings.SenderEmailAddressName);
            message.AddTo(email);
            message.SetTemplateId(Settings.PaswordSendTemplate);
            message.SetTemplateData(new { token });

            var response = await Client.SendEmailAsync(message);
        }
    }
}
