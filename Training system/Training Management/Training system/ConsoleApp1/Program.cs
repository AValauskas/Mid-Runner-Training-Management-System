﻿using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Execute().Wait();
        }
        static async Task Execute()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("aurimas.valauskas@ktu.edu", "Aurimas");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("aurimas19970704@gmail.com", "kitas");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
