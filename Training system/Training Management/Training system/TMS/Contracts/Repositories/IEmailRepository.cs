using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public interface IEmailRepository
    {
        Task SendEmailConfirmationEmail(string email, string token);
        Task SendPasswordResetEmail(string email, string token);
        Task SendNewPassword(string email, string token);
    }
}
